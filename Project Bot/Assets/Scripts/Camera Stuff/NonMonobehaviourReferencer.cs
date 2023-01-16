using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonobehaviourReferencer : MonoBehaviour
{
    public static NonMonobehaviourReferencer instance;

    private void Awake()
    {
        instance = this;
    }

    public void CoroutineTrigger(IEnumerator coroutineToTrigger)
    {
        StartCoroutine(coroutineToTrigger);
    }
}
