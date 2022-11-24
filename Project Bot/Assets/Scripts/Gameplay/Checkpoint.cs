using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 pos;

    private void Awake()
    {
        pos = transform.position;
    }
}
