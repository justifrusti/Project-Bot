using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadCellCheck : MonoBehaviour
{
    public MinigameController controller;

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
    public bool isReferenceCell;
    [Space]
    public Direction direction;
    [Space]
    public Sprite sprite;
    [Space]
    public Vector2Int cellIndex;

    OverloadCellCheck upCell;
    OverloadCellCheck downCell;
    OverloadCellCheck leftCell;
    OverloadCellCheck rightCell;

    private void Start()
    {
        if(!isReferenceCell)
        {
            AssignDirections();
        }
    }

    private void Update()
    {
        if(!isReferenceCell)
        {
            if (isEndCell)
            {
                CheckEndGoal();
            }

            if (direction != Direction.None && !isActive)
            {
                if (upCell != null && upCell.isActive)
                {
                    isActive = true;
                }

                if (downCell != null && downCell.isActive)
                {
                    isActive = true;
                }

                if (rightCell != null && rightCell.isActive)
                {
                    isActive = true;
                }

                if (leftCell != null && leftCell.isActive)
                {
                    isActive = true;
                }
            }
        }
    }

    public void AssignDirections()
    {
        if ((cellIndex.x > 0 && cellIndex.x < 7) && (cellIndex.y > 0 && cellIndex.y < 7))
        {
            upCell = controller.minigameGrid[cellIndex.y - 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
            downCell = controller.minigameGrid[cellIndex.y + 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
            leftCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x - 1].GetComponent<OverloadCellCheck>();
            rightCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x + 1].GetComponent<OverloadCellCheck>();
        }
        else if (cellIndex.x == 0 && cellIndex.y < 7)
        {
            upCell = controller.minigameGrid[cellIndex.y - 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
            downCell = controller.minigameGrid[cellIndex.y + 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
        }
        else if (cellIndex.x < 7 && cellIndex.y == 0)
        {
            leftCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x - 1].GetComponent<OverloadCellCheck>();
            rightCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x + 1].GetComponent<OverloadCellCheck>();
        }
        else if (cellIndex.x == 7 && cellIndex.y < 7)
        {
            upCell = controller.minigameGrid[cellIndex.y - 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
            downCell = controller.minigameGrid[cellIndex.y + 1].row[cellIndex.x].GetComponent<OverloadCellCheck>();
        }
        else if (cellIndex.x < 7 && cellIndex.y == 7)
        {
            leftCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x - 1].GetComponent<OverloadCellCheck>();
            rightCell = controller.minigameGrid[cellIndex.y].row[cellIndex.x + 1].GetComponent<OverloadCellCheck>();
        }
    }

    public void CheckEndGoal()
    {
        if (controller.endPos.y != 0 || controller.endPos.y != 7)
        {
            if (controller.minigameGrid[controller.endPos.y].row[controller.endPos.x - 1].GetComponent<OverloadCellCheck>().isActive)
            {
                controller.minigameGrid[controller.endPos.y].row[controller.endPos.x].GetComponent<OverloadCellCheck>().isActive = true;
            }
            else if (controller.minigameGrid[controller.endPos.y - 1].row[controller.endPos.x].GetComponent<OverloadCellCheck>().isActive)
            {
                controller.minigameGrid[controller.endPos.y].row[controller.endPos.x].GetComponent<OverloadCellCheck>().isActive = true;
            }
            else if (controller.minigameGrid[controller.endPos.y + 1].row[controller.endPos.x].GetComponent<OverloadCellCheck>().isActive)
            {
                controller.minigameGrid[controller.endPos.y].row[controller.endPos.x].GetComponent<OverloadCellCheck>().isActive = true;
            }
        }

        if(isActive)
        {
            controller.FinishMinigame();
        }
    }
}
