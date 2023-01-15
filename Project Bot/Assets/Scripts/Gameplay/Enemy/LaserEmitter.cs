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

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.up, out hit, laserDst))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                manager.playerController.TakeDamage(damage);
            }
        }
    }
}
