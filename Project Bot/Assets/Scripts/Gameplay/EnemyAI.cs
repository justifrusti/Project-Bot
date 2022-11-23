using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]public NavMeshAgent agent;
    [Header("Nav Mesh")]
    public Transform target;

    public Vector3 turnAngle;

    [Header("Enemy Stats")]
    public int health;
    public int damage;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        var targetV3 = new Vector3(target.position.x, target.position.y, target.position.z);

        agent.SetDestination(targetV3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Roomba")
        {
            transform.Rotate(turnAngle);
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ThirdPersonPlayerController>().TakeDamage(damage);
        }
    }
}
