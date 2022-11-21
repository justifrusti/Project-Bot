using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadInitialize : MonoBehaviour
{
    public MinigameController minigame;

    public OverloadingMinigame ovMinigame;

    public void LaunchMinigame()
    {
        minigame.ovMinigame = ovMinigame;
        minigame.minigameType = MinigameController.MinigameType.Overload;

        minigame.InitialSetup();

        this.enabled = false;
    }
}
