using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    public EnemyAI masterTurret;

    public Rigidbody rb;

    public void AssignMasterTurret(EnemyAI ai)
    {
        masterTurret = ai;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        rb.velocity = Vector3.zero;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<ThirdPersonPlayerController>().TakeDamage(masterTurret.damage);
        }

        Destroy(this.gameObject);
    }
}
