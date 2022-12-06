using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseExploder : MonoBehaviour
{
    public GameObject door;

    public float force = 10f;

    public void ExplosiveForce()
    {
        Rigidbody rb = door.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
        rb.AddExplosionForce(force, transform.position, 1);
    }
}
