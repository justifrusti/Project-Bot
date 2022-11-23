using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    public EnemyAI masterTurret;

    public void AssignMasterTurret(EnemyAI ai)
    {
        masterTurret = ai;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ThirdPersonPlayerController>().TakeDamage(masterTurret.damage);
        }

        Destroy(this.gameObject);
    }
}
