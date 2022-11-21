using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigames/Types/Overload", fileName = "New Overload Minigame")]
public class OverloadingMinigame : ScriptableObject
{
    [Header("Ammounts")]
    public int horizontalPieces;
    public int verticalPieces;
    public int downRightPieces;
    public int downLeftPieces;
    public int upRightPieces;
    public int upLeftPieces;
    [Space]
    [Header("Original Ammounts")]
    public int originalHorizontalPieces;
    public int originalVerticalPieces;
    public int originalDownRightPieces;
    public int originalDownLeftPieces;
    public int originalUpRightPieces;
    public int originalUpLeftPieces;
    [Space]
    [Header("Grids")]
    public Array2DBool gridBlockades, gridStartPos, gridEndPos;
}
