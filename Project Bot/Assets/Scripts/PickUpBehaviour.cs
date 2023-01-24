using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickUpBehaviour : MonoBehaviour
{
    public enum Type
    {
        Box,
        ExpBarrel
    }

    public Type type;

    public GameObject particles = null;

    [SerializeField]private ThirdPersonPlayerController playerController = null;
    [SerializeField] private bool expOnImpact;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerController>(); 
    }

    private void Update()
    {
        if(type == Type.ExpBarrel)
        {
            if (playerController.hasPickup && Input.GetButtonDown("LMB") || playerController.hasPickup && Input.GetButtonDown("RMB"))
            {
                expOnImpact = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DeathBox"))
        {
            Destroy(gameObject);
        }

        if(expOnImpact)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 15f);

            foreach(Collider collider in colliders)
            {
                if(collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(150f, transform.position, 15f);

                    GameObject particlesIns = Instantiate(particles, transform.position, Quaternion.identity);

                    playerController.InitilizeShake();

                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathBox"))
        {
            Destroy(gameObject);
        }
    }
}
