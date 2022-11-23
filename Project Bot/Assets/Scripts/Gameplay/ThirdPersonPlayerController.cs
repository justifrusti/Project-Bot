using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{
    public enum MovementMode
    {
        Walking,
        Running,
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
    public bool takeDamage;
    [Space]
    public Transform rayCastPoint;

    RaycastHit hit;

    [Header("Characters Stats")]
    public int health;
    [Space]
    public float invisFramesTime;
    public float dmgKnockback;

    [HideInInspector] public bool invisFramesActive;
    [HideInInspector] public bool canAttack;

    [Header("Character/Cam Movement")]
    public int walkingSpeed;
    public int runSpeed;
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
    
    [Header("Unlocked Abilities")]
    public bool unlockedChargeJump;

    void Start()
    {
        turnSensitivity = normalTurnSensitivity;

        speed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;

        if(unlockedChargeJump)
        {
            jumpMode = JumpMode.ChargeJump;
        }
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

            transform.Translate(move * Time.deltaTime * speed);
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
        if (Input.GetButtonDown("LMB") && hasPickup == true)
        {
            hasPickup = false;

            pickUpItem.GetComponent<Rigidbody>().isKinematic = false;
            pickUpItem.transform.parent = null;

            pickUpItem.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce);
        }

        if (Input.GetButtonDown("RMB") && hasPickup == true)
        {
            hasPickup = false;

            pickUpItem.GetComponent<Rigidbody>().isKinematic = false;
            pickUpItem.transform.parent = null;

            pickUpItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        }

        //Debug
        if (takeDamage)
        {
            TakeDamage();
        }

        Debug.DrawLine(rayCastPoint.position , rayCastPoint.position + rayCastPoint.forward * 2, Color.red, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            timesJumped = 0;

            canJump = true;
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

    public void TakeDamage()
    {
        if(!invisFramesActive)
        {
            CheckHealth(10);

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
                pickUpItem.transform.position = pickUpPoint.transform.position;
                pickUpItem.transform.parent = pickUpPoint.transform;

                hasPickup = true;
            }
        }
    }

    IEnumerator ResetInvis()
    {
        invisFramesActive = true;

        yield return new WaitForSeconds(invisFramesTime);

        invisFramesActive = false;
    }
}
