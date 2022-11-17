using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigames/Types/Overload", fileName = "New Overload Minigame")]
public class OverloadingMinigame : ScriptableObject
{
    public int horizontalPieces;
    public int verticalPieces;
    public int downRightPieces;
    public int downLeftPieces;
    public int upRightPieces;
    public int upLeftPieces;

    public Array2DBool gridBlockades, gridStartPos, gridEndPos;
}
