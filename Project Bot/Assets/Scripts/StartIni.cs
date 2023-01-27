using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIni : MonoBehaviour
{
    public GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}
