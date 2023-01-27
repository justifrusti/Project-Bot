using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicSwitcher : MonoBehaviour
{
    public AudioSource source;

    public AudioClip clipToPlay;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (source.clip != clipToPlay)
            {
                source.clip = clipToPlay;

                source.Play();
                source.loop = true;
            }
        }
    }
}
