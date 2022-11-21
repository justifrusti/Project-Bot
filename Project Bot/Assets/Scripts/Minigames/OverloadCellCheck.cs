using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadCellCheck : MonoBehaviour
{
    public enum Direction
    { 
        None,
        Horizontal,
        Vertical,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }

    public bool isStartCell;
    public bool isEndCell;
    public bool isActive;
    public bool isDisabled;
    [Space]
    public Direction direction;
    [Space]
    public Sprite sprite;
    [Space]
    public Vector2Int cellIndex;

    private void Update()
    {
        if(isEndCell && isActive)
        {
            print("Finish Function");
        }
    }
}
