using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIType
    {
        Roomba,
        Turret
    }

    public AIType type;

    [HideInInspector]public NavMeshAgent agent;
    [Header("Nav Mesh")]
    public Transform target;
    public Transform player;
    [Space]
    public Vector3 turnAngle;

    [Header("Enemy Stats")]
    public int health;
    public int damage;
    [Space]
    public float range;

    [HideInInspector] public bool followPlayer;
    [HideInInspector] public bool stopCountdownStarted;

    [Header("Turret")]
    public Transform barrelRotPoint;
    public Transform bulletPoint;
    [Space]
    public Transform bullet;
    [Space]
    public float fireSpeed;

    public float shootDelay;

    /*[HideInInspector] */public bool canShoot;

    [Header("Debug")]
    public bool showGizmos;

    void Start()
    { 
        switch(type)
        {
            case AIType.Roomba:
                agent = GetComponent<NavMeshAgent>();
                break;

            case AIType.Turret:
                canShoot = true;
                break;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch(type)
        {
            case AIType.Roomba:
                if (!followPlayer)
                {
                    var targetV3 = new Vector3(target.position.x, target.position.y, target.position.z);

                    agent.SetDestination(targetV3);
                }
                else if (followPlayer)
                {
                    agent.SetDestination(player.position);
                }

                CheckFollow();
                break;

            case AIType.Turret:
                PlayerInRange();
                break;
        }
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

    public void PlayerInRange()
    {
        float dstToPlayer = Vector3.Distance(player.position, transform.position);

        if(dstToPlayer <= range)
        {
            barrelRotPoint.LookAt(player);

            if(canShoot)
            {
                canShoot = false;

                StartCoroutine(Shoot());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall")  || collision.gameObject.CompareTag("Roomba"))
        {
            transform.Rotate(turnAngle);
        }

        if(collision.gameObject.CompareTag("Player"))
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

    IEnumerator Shoot()
    {
        Transform spawnedBullet = Instantiate(bullet, bulletPoint.position, Quaternion.identity);

        spawnedBullet.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * fireSpeed, ForceMode.Impulse);
        spawnedBullet.GetComponent<TurretBullet>().AssignMasterTurret(this);

        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    } 
}
