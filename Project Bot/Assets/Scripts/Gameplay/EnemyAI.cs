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
    public Transform player;

    public Vector3 turnAngle;

    [Header("Enemy Stats")]
    public int health;
    public int damage;

    public float range;

    [HideInInspector] public bool followPlayer;
    [HideInInspector] public bool stopCountdownStarted;

    [Header("Debug")]
    public bool showGizmos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(!followPlayer)
        {
            var targetV3 = new Vector3(target.position.x, target.position.y, target.position.z);

            agent.SetDestination(targetV3);
        }else if(followPlayer)
        {
            agent.SetDestination(player.position);
        }

        CheckFollow();
    }

    public void CheckFollow()
    {
        float dstToPlayer = Vector3.Distance(player.position, transform.position);

        if(dstToPlayer <= range)
        {
            followPlayer = true;
        }else if(dstToPlayer > range && followPlayer && !stopCountdownStarted)
        {
            stopCountdownStarted = true;

            StartCoroutine(ReturnTime());
        }
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

    IEnumerator ReturnTime()
    {
        yield return new WaitForSeconds(3);

        followPlayer = false;
        stopCountdownStarted = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    } 
}
