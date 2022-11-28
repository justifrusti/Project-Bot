using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 pos;

    public Transform hubPos;

    private void Awake()
    {
        pos = hubPos.position;
    }
}
