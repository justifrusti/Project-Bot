using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSoundScript : MonoBehaviour
{
    public AudioSource collisionsound;

    private void OnCollisionEnter(Collision collision)
    {
        collisionsound.Play ();
        collisionsound.pitch = Random.Range(0.8f, 1.2f);
    }
}
