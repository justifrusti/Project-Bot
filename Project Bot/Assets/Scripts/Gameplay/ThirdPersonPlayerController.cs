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

    public MovementMode movementMode;

    [Header("Debug and Non Catogarized")]
    public Rigidbody rb;

    public bool takeDamage;

    [Header("Characters Stats")]
    public int health;

    public float invisFramesTime;
    public float dmgKnockback;

    [HideInInspector] public bool invisFramesActive;
    [HideInInspector] public bool canAttack;

    [Header("Character/Cam Movement")]
    public int walkingSpeed;
    public int runSpeed;
    public int maxJumps;

    public float normalTurnSensitivity;
    public float lockedTurnSensitivity;
    public float runTurnSensitivity;

    public Vector3 jump;

    [HideInInspector] public int speed;
    [HideInInspector] public int timesJumped;
    [HideInInspector] public float turnSensitivity;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool canJump;

    void Start()
    {
        turnSensitivity = normalTurnSensitivity;

        speed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        //Player Movement
        if(isMoving)
        {
            MoveCharacter();
        }

        if (Input.GetButton("Horizontal"))
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

        //Jump
        if(Input.GetButtonDown("Jump"))
        {
            if(timesJumped == maxJumps)
            {
                canJump = false;
            }

            Jump();
        }

        if(takeDamage)
        {
            TakeDamage();
        }
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
        float h = Input.GetAxis("Horizontal");

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

                move.x = h;
                move.z = v;

                transform.Translate(move * Time.deltaTime * speed);
                break;

            case MovementMode.Running:
                speed = runSpeed;
                turnSensitivity = runTurnSensitivity;

                move.x = h;
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

    IEnumerator ResetInvis()
    {
        invisFramesActive = true;

        yield return new WaitForSeconds(invisFramesTime);

        invisFramesActive = false;
    }
}
