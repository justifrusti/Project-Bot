using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float adjustSpeed;

    public Transform moveTowards;

    public Vector3 originalPos;

    [HideInInspector] public bool onPressureplate;

    private void Awake()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if(!onPressureplate)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, adjustSpeed * Time.deltaTime);
        }else if(onPressureplate)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowards.position, adjustSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ActivatePlate"))
        {
            print("Plate Active");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onPressureplate = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        onPressureplate = false;
    }
}
