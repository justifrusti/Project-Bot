using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountManager : MonoBehaviour
{
    public GameManager manager;

    public TMP_Text text;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = manager.saveData.deaths.ToString();
    }
}
