using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkAttack : MonoBehaviour
{
    public List<GameObject> sparkObjs;

    private void OnTriggerEnter(Collider other)
    {
        print("E");

        /*if(other.gameObject.CompareTag("FuseBox") || other.gameObject.CompareTag("Enemy"))
        {
            if(!sparkObjs.Contains(other.gameObject))
            {
                sparkObjs.Add(other.gameObject);
            }
        }*/

        if (!sparkObjs.Contains(other.gameObject))
        {
            sparkObjs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
