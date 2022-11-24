using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{
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

    public MovementMode movementMode;
    [Space]
    public JumpMode jumpMode;

    [Header("Debug and Non Catogarized")]
    public Rigidbody rb;
    [Space]
    public Transform rayCastPoint;
    [Space]
    public Transform cmCam;

    RaycastHit hit;

    [Header("Characters Stats")]
    public int health;
    public int damage;
    [Space]
    public float invisFramesTime;
    public float dmgKnockback;

    [HideInInspector] public bool invisFramesActive;
    [HideInInspector] public bool canAttack;

    [Header("Character/Cam Movement")]
    public int walkingSpeed;
    public int runSpeed;
    public int speedPadSpeed;
    [SerializeField] private int maxJumps;
    [Space]
    public float normalTurnSensitivity;
    public float lockedTurnSensitivity;
    public float runTurnSensitivity;
    [Space]
    [SerializeField] private float maxJumpCharge;
    public float jumpChargeSpeed;
    [Space]
    public Vector3 jump;
    [Space]
    public Vector3 jumpPadJump;
    [SerializeField] private Vector3 originalJump;
    [Space]
    [SerializeField] private int originalWalkSpeed;
    [SerializeField] private int originalRunSpeed;

    [HideInInspector] public int speed;
    [HideInInspector] public int timesJumped;
    [HideInInspector] public float turnSensitivity;
    [HideInInspector] public float jumpCharge;
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

    void Start()
    {
        currentActiveCheckpoint = transform.position;

        turnSensitivity = normalTurnSensitivity;

        speed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;

        //SkillTreeReader.Instance.availablePoints = manager.saveData.skillPoints;
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
            TurnChar();
        }else if(Input.GetButton("Horizontal") && !isMoving && !turnMode)
        {
            Vector3 move = new Vector3();

            float h = Input.GetAxis("Horizontal");
            move.x = h;

            transform.Translate(move * Time.deltaTime * speed, cmCam);
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
        if(Input.GetKeyDown(KeyCode.Escape))
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

        //Raycast Length
        /*Debug.DrawLine(rayCastPoint.position , rayCastPoint.position + rayCastPoint.forward * 2, Color.red, 1.0f);*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("JumpPad"))
        {
            timesJumped = 0;

            canJump = true;
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
        }

        if(collision.gameObject.CompareTag("JumpPad"))
        {
            jump = jumpPadJump;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpPad"))
        {
            jump = originalJump;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("SpeedPad"))
        {
            movementMode = MovementMode.SpeedPad;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            currentActiveCheckpoint = other.gameObject.GetComponent<Checkpoint>().pos;
        }

        if(other.gameObject.CompareTag("DeathBox"))
        {
            transform.position = currentActiveCheckpoint;
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

                transform.Translate(move * Time.deltaTime * speed);
                break;

            case MovementMode.Running:
                speed = runSpeed;
                turnSensitivity = runTurnSensitivity;

                move.z = v;

                transform.Translate(move * Time.deltaTime * speed);
                break;

            case MovementMode.SpeedPad:
                speed = speedPadSpeed;
                turnSensitivity = runTurnSensitivity;

                move.z = v;

                transform.Translate(move * Time.deltaTime * speed);
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

    public void DoDamage()
    {
        if (canAttack)
        {
            canAttack = false;

            print("Enemy Took Damage");

            /*currentEnemy.TakeDamage(currentWeapon.damage);

            Vector3 dir = (currentEnemy.transform.position - transform.position).normalized;
            currentEnemy.rb.AddForce(dir * currentEnemy.dmgKnockback, ForceMode.Impulse);*/
        }
    }

    public void TakeDamage(int damage)
    {
        if(!invisFramesActive)
        {
            CheckHealth(damage);

            KnockBack();

            StartCoroutine(ResetInvis());
        }
    }

    public void CheckHealth(int damage)
    {
        if ((health -= damage) <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void KnockBack()
    {
        rb.velocity = Vector3.zero;

        Vector3 dir = (transform.position - (transform.forward * 2)).normalized;
        rb.AddForce(dir * dmgKnockback, ForceMode.Impulse);
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
            damage = damage * 6;
        }else if (SkillTreeReader.Instance.IsSkillUnlocked(4))
        {
            damage = damage * 4;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(1))
        {
            damage = damage * 2;
        }

        if (SkillTreeReader.Instance.IsSkillUnlocked(8))
        {
            health = health * 6;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(5))
        {
            health = health * 4;
        }
        else if (SkillTreeReader.Instance.IsSkillUnlocked(2))
        {
            health = health * 2;
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
        }
    }
}
