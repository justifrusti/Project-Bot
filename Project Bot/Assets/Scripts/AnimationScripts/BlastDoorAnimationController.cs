using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDoorAnimationController : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartOpeningAnim()
    {
        animator.SetTrigger("isOpening");
    }

    public void StartClosingAnim()
    {
        animator.SetTrigger("isClosing");
    }
}
