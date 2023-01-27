using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadInitialize : MonoBehaviour
{
    public MinigameController minigame;

    public OverloadingMinigame ovMinigame;

    public GameObject objToDeactivate;

    public void LaunchMinigame()
    {
        minigame.ovMinigame = ovMinigame;

        ResetValues();

        minigame.AssignController();
        minigame.InitialSetup();

        this.enabled = false;
    }

    public void ResetValues()
    {
        ovMinigame.horizontalPieces = ovMinigame.originalHorizontalPieces;
        ovMinigame.verticalPieces = ovMinigame.originalVerticalPieces;
        ovMinigame.upLeftPieces = ovMinigame.originalUpLeftPieces;
        ovMinigame.upRightPieces = ovMinigame.originalUpRightPieces;
        ovMinigame.downLeftPieces = ovMinigame.originalDownLeftPieces;
        ovMinigame.downRightPieces = ovMinigame.originalDownRightPieces;
    }
}
