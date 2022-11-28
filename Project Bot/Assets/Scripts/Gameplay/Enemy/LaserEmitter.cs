using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LaserEmitter : MonoBehaviour
{
    public GameManager manager;
    [Space]
    public int damage;
    public float laserDst;

    RaycastHit hit;

    private void Start()
    {

    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.right, out hit, laserDst))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                manager.playerController.TakeDamage(damage);
            }
        }
    }
}
