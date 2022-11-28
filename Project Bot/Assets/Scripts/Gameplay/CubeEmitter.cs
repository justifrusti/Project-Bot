using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEmitter : MonoBehaviour
{
    public GameObject cubeToEmit;
    GameObject cube;

    public Transform emitLocation;

    private void Update()
    {
        if(cube == null)
        {
            cube = Instantiate(cubeToEmit, emitLocation.position, Quaternion.identity);
        }
    }
}
