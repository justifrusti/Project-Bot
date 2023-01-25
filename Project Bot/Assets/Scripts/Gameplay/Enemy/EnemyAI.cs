using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;

public class EnemyAI : MonoBehaviour
{
    public enum AIType
    {
        Roomba,
        Turret,
        RoombaGun
    }

    public enum RoombaType
    {
        Default,
        Gun,
        SuicideRoomba
    }

    public AIType type;
    public RoombaType roombaType;
    [HideInInspector]public NavMeshAgent agent;
    [Header("Nav Mesh")]
    public Transform target;
    public Transform player;
    [Space]
    public Vector3 turnAngle;

    [Header("Enemy Stats")]
    public float health;
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

    [HideInInspector] public bool canShoot;

    [Header("Visuals")]
    public GameObject exParticles;

    private GameObject spawnedParticles;

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

            case AIType.RoombaGun:
                agent = GetComponent<NavMeshAgent>();

                canShoot = true;
                break;
        }

        player = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
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

            case AIType.RoombaGun:
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
            if (type != AIType.RoombaGun)
            {
                barrelRotPoint.LookAt(player);
            }

            if(canShoot)
            {
                canShoot = false;

                StartCoroutine(Shoot());
            }
        }
    }

    /*public void KnockBack()
    {
        Vector3 dir = (transform.position - (transform.forward * -1)).normalized;
        rb.AddForce(dir * dmgKnockback, ForceMode.Impulse);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (type == AIType.Roomba)
            {
                //KnockBack();
                if(roombaType == RoombaType.SuicideRoomba)
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, range);

                    foreach (Collider collider in colliders)
                    {
                        if (collider.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(150f, transform.position, range);

                            spawnedParticles = Instantiate(exParticles, transform.position, Quaternion.identity);

                            Destroy(gameObject);
                        }
                    }
                }
            }

            collision.gameObject.GetComponent<ThirdPersonPlayerController>().TakeDamage(damage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Roomba"))
        {
            transform.Rotate(turnAngle * Time.deltaTime);
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

    public void CheckHealth(float damage)
    {
        if ((health -= damage) <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        spawnedParticles = Instantiate(exParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
