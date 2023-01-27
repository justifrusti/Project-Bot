using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CameraSwitcher.Register(cam);
            CameraSwitcher.SwitchCamera(cam);
        }
    }
}
