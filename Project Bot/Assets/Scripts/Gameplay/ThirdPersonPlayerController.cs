using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FacialExpressionManager))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    public GameManager manager;
    
    public enum MovementMode
    {
        Walking,
        Running,
        SpeedPad,
        Idle
    }

    public enum JumpMode
    {
        NormalJump,
        ChargeJump
    }

    public enum CamMode
    {
        FreeLook,
        ThirdPerson
    }

    public MovementMode movementMode;
    [Space]
    public JumpMode jumpMode;
    [Space]
    public CamMode camMode;

    [Header("Debug and Non Catogarized")]
    public Rigidbody rb;
    [Space]
    public Transform rayCastPoint;
    [Space]
    public Transform cam;
    public CinemachineFreeLook cmCam;
    [Space]
    public Transform rightHand;
    [Space]
    public Transform wheels;
    public Transform head;

    private Color emissionColor;
    private Material currentMat;
    private bool updatedMat;

    RaycastHit hit;

    [Header("Characters Stats")]
    public int maxHearts;
    [Space]
    public int maxDamageCharge;
    public float damageChargeSpeed;
    [Space]
    public float invisFramesTime;
    public int chargeShootSpeed;

    public GameObject bullet;

    [HideInInspector] public bool invisFramesActive;
    [HideInInspector] public int hearts;
    [HideInInspector] public float damage;
    [HideInInspector] public int deaths;

    [Header("Character/Cam Movement")]
    public int walkingSpeed;
    public int runSpeed;
    public int speedPadSpeed;
    public int maxJumps;
    [Space]
    public float normalTurnSensitivity;
    public float lockedTurnSensitivity;
    public float runTurnSensitivity;
    [Space]
    public float maxJumpCharge;
    public float jumpChargeSpeed;
    [Space]
    public Vector3 jump;
    [Space]
    public Vector3 jumpPadJump;
    public Vector3 originalJump;
    [Space]
    public int originalWalkSpeed;
    public int originalRunSpeed;
    [HideInInspector] public int speed;
    [HideInInspector] public int timesJumped;
    [HideInInspector] public float turnSensitivity;
    [HideInInspector] public float jumpCharge;
    [HideInInspector] public float camSens;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool canJump;
    [HideInInspector] public bool turnMode;
    [Space]
    [Header("Pick Ups")]
    public Transform pickUpPoint;
    [Space]
    public float pickUpRayDst;
    public float throwForce;
    public float dropForce;

    [HideInInspector] public GameObject pickUpItem;
    [HideInInspector] public bool hasPickup;

    [HideInInspector] public Vector3 currentActiveCheckpoint;

    [Header("Skills")]
    public GameObject skillUI;

    [Header("KeyCards")]
    public int keyCardAmmounts;

    public bool unlockedBlueKK;
    public bool unlockedYellowKK;
    public bool unlockedGreeKK;
    public bool unlockedRedKK;

    //Private Check Variables
    private bool onPad;

    private bool canChangeEmotion = true;

    void Start()
    {
        VariableSetup();

        Cursor.lockState = CursorLockMode.Locked;

        if(camMode == CamMode.ThirdPerson)
        {
            cmCam.m_XAxis.m_MaxSpeed = 0;
        }

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        SkillTreeReader.Instance.availablePoints = manager.saveData.skillPoints;
    }

    private void FixedUpdate()
    {
        //Player Movement
        if(isMoving)
        {
            MoveCharacter();
        }

        if (Input.GetButton("Horizontal") && isMoving)
        {
            TurnChar();
        }else if(Input.GetButton("Horizontal") && !isMoving && turnMode)
        {
            float h = Input.GetAxis("Horizontal");

            Vector3 rightWheel = new Vector3(0, 180, 0);
            Vector3 leftWheel = new Vector3(0, -180, 0);

            if (h > 0)
            {
                wheels.Rotate(rightWheel * Time.deltaTime, Space.Self);
            }
            else if (h < 0)
            {
                wheels.Rotate(leftWheel * Time.deltaTime, Space.Self);
            }
        }
        else if(Input.GetButton("Horizontal") && !isMoving && !turnMode)
        {
            TurnChar();
        }
    }

    void Update()
    {
        //Player Movement
        if (Input.GetButtonDown("Vertical"))
        {
            isMoving = true;
            movementMode = MovementMode.Walking;
        }
        else if (Input.GetButtonUp("Vertical"))
        {
            isMoving = false;
            movementMode = MovementMode.Idle;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            turnMode = true;
        }else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            turnMode = false;
        }

        if(movementMode == MovementMode.Idle && canChangeEmotion)
        {
            manager.facialManager.ChangeEM(false, 0, FacialExpressionManager.CurrentExpression.Default);
        }

        //Jump
        switch(jumpMode)
        {
            case JumpMode.NormalJump:
                if (Input.GetButtonDown("Jump"))
                {
                    if (timesJumped == maxJumps)
                    {
                        canJump = false;
                    }

                    Jump();
                }
                break;

            case JumpMode.ChargeJump:
                if (Input.GetButton("Jump"))
                {
                    if (timesJumped == maxJumps)
                    {
                        canJump = false;
                    }

                    ChargedJump();
                }else if(Input.GetButtonUp("Jump"))
                {
                    if (timesJumped != maxJumps)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, 0);
                        rb.velocity += new Vector3(0, jump.y + jumpCharge, 0);

                        timesJumped++;
                    }

                    if (timesJumped == maxJumps)
                    {
                        canJump = false;
                    }

                    jumpCharge = 0;
                }
                break;
        }

        if(Physics.Raycast(rayCastPoint.position, rayCastPoint.forward, out hit, 2f))
        {
            if(hit.collider.tag == "Overload")
            {
                if(Input.GetButtonDown("Interact"))
                {
                    hit.collider.GetComponent<OverloadInitialize>().LaunchMinigame();
                }
            }

            if (hit.collider.gameObject.tag == "PickUp")
            {
                HoldablePickUp();
            }
        }

        //Pickup
        if (Input.GetButtonDown("LMB") && hasPickup)
        {
            hasPickup = false;

            pickUpItem.GetComponent<Collider>().isTrigger = false;

            pickUpItem.GetComponent<Rigidbody>().isKinematic = false;
            pickUpItem.transform.parent = null;

            pickUpItem.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce);
        }

        if (Input.GetButtonDown("RMB") && hasPickup)
        {
            hasPickup = false;

            pickUpItem.GetComponent<Collider>().isTrigger = false;

            pickUpItem.GetComponent<Rigidbody>().isKinematic = false;
            pickUpItem.transform.parent = null;

            pickUpItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        }

        if(Input.GetButtonDown("Respawn"))
        {
            transform.position = currentActiveCheckpoint;
        }

        //Skills
        if(Input.GetButtonDown("Esc"))
        {
            if(skillUI.activeInHierarchy)
            {
                skillUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }else
            {
                skillUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }

        //Cam
        if(Input.GetButtonDown("CamMode"))
        {
            if(camMode == CamMode.FreeLook)
            {
                camMode = CamMode.ThirdPerson;
                cmCam.m_XAxis.m_MaxSpeed = 0;
            }else if(camMode == CamMode.ThirdPerson)
            {
                camMode = CamMode.FreeLook;
                cmCam.m_XAxis.m_MaxSpeed = camSens;
            }
        }

        //Charge Shot
        if(Input.GetButton("LMB"))
        {
            if (damage < maxDamageCharge)
            {
                damage += damageChargeSpeed * Time.deltaTime;
            }
            else if (damage > maxDamageCharge)
            {
                damage = maxDamageCharge;
            }
        }

        if(Input.GetButtonUp("LMB"))
        {
            GameObject currentBullet = Instantiate(bullet, rightHand.position, Quaternion.identity);

            currentBullet.GetComponent<Rigidbody>().AddForce(rightHand.up * chargeShootSpeed, ForceMode.Impulse);
            currentBullet.GetComponent<PlayerBullet>().AssignPlayer(this);

            manager.facialManager.ChangeEM(true, .5f, FacialExpressionManager.CurrentExpression.Wink);
            canChangeEmotion = false;
            StartCoroutine(ResetBool(.5f));
        }

        //Debug
        //Debug.DrawLine(rayCastPoint.position, rayCastPoint.forward * 1,Color.red, 1.0f);

        if (Input.GetKeyDown(KeyCode.V))
        {
            deaths = manager.saveData.deaths;
            deaths++;
            manager.saveData.deaths = deaths;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("JumpPad"))
        {
            timesJumped = 0;

            canJump = true;

            if(collision.gameObject.CompareTag("Floor") && canChangeEmotion)
            {
                manager.facialManager.ChangeEM(true, .5f, FacialExpressionManager.CurrentExpression.Shocked2);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(.5f));
            }
        }

        if(collision.gameObject.CompareTag("Floor") && movementMode == MovementMode.SpeedPad)
        {
            if (isRunning)
            {
                movementMode = MovementMode.Running;
            }
            else
            {
                movementMode = MovementMode.Walking;
            }

            timesJumped = 0;

            canJump = true;

            onPad = false;
        }

        if(collision.gameObject.CompareTag("JumpPad"))
        {
            jump = jumpPadJump;
        }

        if(collision.gameObject.CompareTag("KeyCard"))
        {
            switch(collision.gameObject.GetComponent<KeyCardController>().color)
            {
                case KeyCardController.KeyCardColor.Blue:
                    unlockedBlueKK = true;
                    break;

                case KeyCardController.KeyCardColor.Red:
                    unlockedRedKK = true;
                    break;

                case KeyCardController.KeyCardColor.Green:
                    unlockedGreeKK = true;
                    break;

                case KeyCardController.KeyCardColor.Yellow:
                    unlockedYellowKK = true;
                    break;
            }

            manager.uiManager.KeyCardIndicators();

            Destroy(collision.gameObject);
        }

        if(collision.gameObject.CompareTag("KeyDoor"))
        {
            if (collision.gameObject.GetComponent<KeyCardController>().color == KeyCardController.KeyCardColor.Blue && unlockedBlueKK)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }else if (collision.gameObject.GetComponent<KeyCardController>().color == KeyCardController.KeyCardColor.Red && unlockedRedKK)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }
            else if(collision.gameObject.GetComponent<KeyCardController>().color == KeyCardController.KeyCardColor.Green && unlockedGreeKK)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }else if (collision.gameObject.GetComponent<KeyCardController>().color == KeyCardController.KeyCardColor.Yellow && unlockedYellowKK)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }else
            {
                manager.facialManager.ChangeEM(true, 1.5f, FacialExpressionManager.CurrentExpression.Death3);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(1.5f));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            jump = originalJump;

            if(!canJump)
            {
                FindObjectOfType<AudioManagerScript>().Play("jumpBoost");

                if(canChangeEmotion)
                {
                    canChangeEmotion = false;

                    manager.facialManager.ChangeEM(true, 2, FacialExpressionManager.CurrentExpression.OverJoyed);

                    ResetBool(2);
                }
            }
        }

        if(collision.gameObject.CompareTag("SpeedPad"))
        {
            currentMat.SetColor("_EmissionColor", emissionColor);
            updatedMat = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("SpeedPad"))
        {
            if(!updatedMat)
            {
                updatedMat = true;

                currentMat = collision.gameObject.GetComponent<MeshRenderer>().material;
                emissionColor = currentMat.GetColor("_EmissionColor");

                currentMat.SetColor("_EmissionColor", Color.black);
            }

            movementMode = MovementMode.SpeedPad;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            Vector3 respawnPos = other.gameObject.GetComponent<Checkpoint>().pos;
            Vector3 alteredRespawnPos = new Vector3(respawnPos.x, respawnPos.y + 1, respawnPos.z + 1);

            currentActiveCheckpoint = alteredRespawnPos;

            if(canChangeEmotion)
            {
                manager.facialManager.ChangeEM(true, 3f, FacialExpressionManager.CurrentExpression.Smug);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(3f));
            }
        }

        if(other.gameObject.CompareTag("Teleporter"))
        {
            Vector3 respawnPos = other.gameObject.GetComponent<Teleporter>().pos;
            Vector3 alteredRespawnPos = new Vector3(respawnPos.x, respawnPos.y + 1, respawnPos.z + 1);

            transform.position = alteredRespawnPos;

            if(canChangeEmotion)
            {
                manager.facialManager.ChangeEM(true, 3f, FacialExpressionManager.CurrentExpression.Smile);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(3f));
            }    
        }

        if(other.gameObject.CompareTag("DeathBox"))
        {
            transform.position = currentActiveCheckpoint;

            int randomizer = Random.Range(0, 3);

            if(randomizer == 0)
            {
                manager.facialManager.ChangeEM(true, 3f, FacialExpressionManager.CurrentExpression.Death1);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(3));
            }
            else if(randomizer == 1)
            {
                manager.facialManager.ChangeEM(true, 3f, FacialExpressionManager.CurrentExpression.Death2);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(3));
            }
            else if(randomizer == 2)
            {
                manager.facialManager.ChangeEM(true, 3f, FacialExpressionManager.CurrentExpression.Death3);
                canChangeEmotion = false;
                StartCoroutine(ResetBool(3));
            }
        }
    }

    public void MoveCharacter()
    {
        Vector3 move = new Vector3();

        float v = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Sprint"))
        {
            movementMode = MovementMode.Running;

            isRunning = true;
        }else if(Input.GetButtonUp("Sprint"))
        {
            movementMode = MovementMode.Walking;

            isRunning = false;
        }

        switch (movementMode)
        {
            case MovementMode.Walking:
                speed = walkingSpeed;
                turnSensitivity = normalTurnSensitivity;

                move.z = v;

                transform.Translate(move * Time.deltaTime * speed, wheels);

                if(canChangeEmotion)
                {
                    manager.facialManager.ChangeEM(false, 0, FacialExpressionManager.CurrentExpression.Happy);
                }
                break;

            case MovementMode.Running:
                speed = runSpeed;
                turnSensitivity = runTurnSensitivity;

                move.z = v;

                transform.Translate(move * Time.deltaTime * speed, wheels);

                if(canChangeEmotion)
                {
                    manager.facialManager.ChangeEM(false, 0, FacialExpressionManager.CurrentExpression.Happy);
                }
                break;

            case MovementMode.SpeedPad:
                if (!onPad)
                {
                    FindObjectOfType<AudioManagerScript>().Play("speedBoost");
                }

                onPad = true;

                speed = speedPadSpeed;
                turnSensitivity = runTurnSensitivity;

                move.z = v;

                transform.Translate(move * Time.deltaTime * speed, wheels);

                if(canChangeEmotion)
                {
                    manager.facialManager.ChangeEM(false, 0, FacialExpressionManager.CurrentExpression.Shocked1);
                }
                break;
        }
    }

    //Turning the character's body left and right
    public void TurnChar()
    {
        Vector3 rotateBody = new Vector3();

        float turnY = Input.GetAxis("Horizontal");

        rotateBody.y = turnY;

        transform.Rotate(rotateBody * Time.deltaTime * turnSensitivity);
    }

    public void Jump()
    {
        if (timesJumped != maxJumps)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0);
            rb.velocity += jump;

            timesJumped++;
        }

        if (timesJumped == maxJumps)
        {
            canJump = false;
        }
    }

    public void ChargedJump()
    {
        if (jumpCharge < maxJumpCharge)
        {
            jumpCharge += jumpChargeSpeed * Time.deltaTime;
        }

        if (jumpCharge > maxJumpCharge)
        {
            jumpCharge = maxJumpCharge;
        }
    }

    public void DoDamage(float damage)
    {
        print("Enemy Took Damage");

        /*if(collision.gameObject.transform.parent.gameObject.GetComponent<EnemyAI>() != null)
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<EnemyAI>().CheckHealth(damage);
        }else if(collision.gameObject.GetComponent<EnemyAI>() != null)
        {
            collision.gameObject.GetComponent<EnemyAI>().CheckHealth(damage);
        }*/
    }

    public void TakeDamage(int damage)
    {
        if(!invisFramesActive)
        {
            CheckHealth(damage);

            StartCoroutine(ResetInvis());
        }
    }

    public void CheckHealth(int damage)
    {
        if ((hearts -= damage) <= 0)
        {
            Die();
        }else
        {
            canChangeEmotion = false;
            StartCoroutine(ResetBool(1.2f));

            int randomizer = Random.Range(0, 3);

            if(randomizer == 0)
            {
                manager.facialManager.ChangeEM(true, 1.2f, FacialExpressionManager.CurrentExpression.Pain1);
            }else if(randomizer == 1)
            {
                manager.facialManager.ChangeEM(true, 1.2f, FacialExpressionManager.CurrentExpression.Pain2);
            }else if(randomizer == 2)
            {
                manager.facialManager.ChangeEM(true, 1.2f, FacialExpressionManager.CurrentExpression.Pain3);
            }else
            {
                manager.facialManager.ChangeEM(true, 1.2f, FacialExpressionManager.CurrentExpression.Pain1);
            }
        }
    }

    public void Die()
    {
        deaths = manager.saveData.deaths;
        deaths++;
        manager.saveData.deaths = deaths;
        Destroy(gameObject);
    }

    public void HoldablePickUp()
    {
        print("Press E to PickUp and Hold");

        if (hit.transform.tag == "PickUp" && !hasPickup)
        {
            pickUpItem = hit.collider.gameObject;

            if (Input.GetButtonDown("Interact"))
            {
                pickUpItem.GetComponent<Rigidbody>().isKinematic = true;
                pickUpItem.GetComponent<Collider>().isTrigger = true;
                pickUpItem.transform.position = pickUpPoint.transform.position;
                pickUpItem.transform.parent = pickUpPoint.transform;

                hasPickup = true;
            }
        }
    }

    public void RespawnAtCheckpoint()
    {
        transform.position = currentActiveCheckpoint;
    }

    IEnumerator ResetInvis()
    {
        invisFramesActive = true;

        yield return new WaitForSeconds(invisFramesTime);

        invisFramesActive = false;
    }

    public void ActivateUnlockedSkills()
    {
        if(SkillTreeReader.Instance.IsSkillUnlocked(7))
        {
            maxDamageCharge = 30;
            damageChargeSpeed = 10;
        }else if (SkillTreeReader.Instance.IsSkillUnlocked(4))
        {
            maxDamageCharge = 20;
            damageChargeSpeed = 6;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(1))
        {
            maxDamageCharge = 15;
            damageChargeSpeed = 4;
        }

        if (SkillTreeReader.Instance.IsSkillUnlocked(8))
        {
            maxHearts = 6;
            hearts = maxHearts;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(5))
        {
            maxHearts = 5;
            hearts = maxHearts;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(2))
        {
            maxHearts = 4;
            hearts = maxHearts;
        }

        if (SkillTreeReader.Instance.IsSkillUnlocked(9))
        {
            jumpMode = JumpMode.ChargeJump;
            jumpCharge = jumpCharge * 6;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(6))
        {
            jumpMode = JumpMode.ChargeJump;
            jumpCharge = jumpCharge * 4;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(3))
        {
            jumpMode = JumpMode.ChargeJump;
            jumpCharge = jumpCharge * 2;
        }else if(SkillTreeReader.Instance.IsSkillUnlocked(12))
        {
            chargeShootSpeed = 20;
        }else if(SkillTreeReader.Instance.IsSkillUnlocked(11))
        {
            chargeShootSpeed = 15;
        }else if(SkillTreeReader.Instance.IsSkillUnlocked(10))
        {
            chargeShootSpeed = 10;
        }
    }

    public void VariableSetup()
    {
        currentActiveCheckpoint = transform.position;

        turnSensitivity = normalTurnSensitivity;

        speed = walkingSpeed;

        camSens = cmCam.m_XAxis.m_MaxSpeed;

        hearts = maxHearts;
    }

    IEnumerator ResetBool(float time)
    {
        yield return new WaitForSeconds(time);

        canChangeEmotion = true;
    }
}
