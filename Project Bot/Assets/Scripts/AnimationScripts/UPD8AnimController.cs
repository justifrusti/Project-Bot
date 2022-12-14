using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPD8AnimController : MonoBehaviour
{
    public Animator anim;

    public float minSpecialIdleTime, maxSpecialIdleTime;

    private bool isPlayingSpecial;
    private float timeTillSpecial;
    private bool triggeredEnd;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    IEnumerator ResetTimer(bool boolToTrigger)
    {
        yield return new WaitForSeconds(5);

        boolToTrigger = false;
        isPlayingSpecial = false;
    }
}
