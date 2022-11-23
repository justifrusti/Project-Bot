using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaSpawner : MonoBehaviour
{
    public GameObject roomba;
    public Transform target;

    void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            Instantiate(roomba, target.position, Quaternion.identity);
        }
    }
}
