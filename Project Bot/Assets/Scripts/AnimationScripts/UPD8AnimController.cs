using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPD8AnimController : MonoBehaviour
{
    public Animator anim;

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
    }

    IEnumerator ResetTimer()
    {
        anim.SetTrigger("IdleSpecial1");

        yield return new WaitForSeconds(15);

        anim.SetTrigger("IdleSpecial2");

        yield return new WaitForSeconds(7);

        triggeredEnd = false;
        isPlayingSpecial = false;
    }
}
