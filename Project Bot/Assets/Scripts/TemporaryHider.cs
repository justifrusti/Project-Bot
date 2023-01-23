using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHider : MonoBehaviour
{
    public bool isHidden;
    [Space]
    public GameObject objToHide;
    [Space]
    public float disappearTime;
    public float appearTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!isHidden)
            {
                StartCoroutine(Hider());
            }
        }
    }

    IEnumerator Hider()
    {
        isHidden = true;

        yield return new WaitForSeconds(disappearTime);
        objToHide.SetActive(false);
        yield return new WaitForSeconds(appearTime);
        objToHide.SetActive(true);

        isHidden = false;
    }
}
