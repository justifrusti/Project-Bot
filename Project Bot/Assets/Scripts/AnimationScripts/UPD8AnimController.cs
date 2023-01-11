using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPD8AnimController : MonoBehaviour
{
    public Animator anim;
    public ThirdPersonPlayerController controller;
    [Space]
    public float fallingThreshold;
    [Space]
    public float minSpecialIdleTime, maxSpecialIdleTime;

    private IEnumerator timer;
    private bool isPlayingSpecial;
    private float timeTillSpecial;
    private bool triggeredEnd;
    public static bool playingIdle = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(controller.jumpMode == ThirdPersonPlayerController.JumpMode.ChargeJump)
        {
            anim.SetBool("ChargeJump", true);
        }else
        {
            anim.SetBool("ChargeJump", false);
        }

        if(playingIdle)
        {
            anim.SetBool("PlayingIdle", true);

            if (!isPlayingSpecial)
            {
                isPlayingSpecial = true;

                timeTillSpecial = Random.Range(minSpecialIdleTime, maxSpecialIdleTime);
            }

            if (timeTillSpecial > 0)
            {
                timeTillSpecial -= 1 * Time.deltaTime;
            }
            else if (timeTillSpecial <= 0)
            {
                if (!triggeredEnd)
                {
                    triggeredEnd = true;

                    StartCoroutine(ResetTimer());
                }
            }
        }else
        {
            anim.SetBool("PlayingIdle", false);
        }

        if(Input.GetButtonDown("Jump") && controller.canJump)
        {
            anim.SetTrigger("Jump");
        }

        if(Input.GetButtonUp("Jump") && controller.canJump)
        {
            anim.SetTrigger("JumpUp");
        }else if(Input.GetButtonUp("Jump") && !controller.canJump)
        {
            anim.SetTrigger("JumpUp");
        }

        if(controller.rb.velocity.y < fallingThreshold)
        {
            anim.SetTrigger("JumpDown");
        }

        if(Input.GetButtonDown("LMB"))
        {
            anim.SetTrigger("ShootAim");
        }else if(Input.GetButtonUp("LMB"))
        {
            anim.SetTrigger("ShootRelease");
        }
    }

    IEnumerator ResetTimer()
    {
        anim.SetTrigger("IdleSpecial1");

        yield return new WaitForSeconds(15);

        anim.SetTrigger("IdleSpecial2");

        yield return new WaitForSeconds(7);

        anim.SetTrigger("IdleSpecial3");

        yield return new WaitForSeconds(10);

        triggeredEnd = false;
        isPlayingSpecial = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetTrigger("HitGround");
        }
    }
}
