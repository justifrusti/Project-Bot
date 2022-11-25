using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaveData saveData;
    public ThirdPersonPlayerController playerController;

    public void InitializeScripts()
    {
        playerController = GetComponent<ThirdPersonPlayerController>();
    }
}
