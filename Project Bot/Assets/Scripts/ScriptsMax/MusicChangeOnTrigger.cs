using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeOnTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManagerScript>().StopPlaying("GameplayMusic");
        FindObjectOfType<AudioManagerScript>().Play("naamvangeluidhier");
    }
}
