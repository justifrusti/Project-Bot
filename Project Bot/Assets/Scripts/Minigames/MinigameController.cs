using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    [System.Serializable]
    public struct GridRow
    {
        public List<Button> row;
    }

    public List<GridRow> minigameGrid;
    public GridLayoutGroup layout;

    [HideInInspector]public Vector2Int endPos;

    [Header("Minigame Ref")]
    public OverloadingMinigame ovMinigame;
    [Space]
    public OverloadCellCheck hand;
    [Space]
    [Header("UI Elements")]
    public GameObject ovUI;
    [Space]
    public Image horizontalImg;
    public Image verticalImg;
    public Image downRightImg;
    public Image downLeftImg;
    public Image upRightImg;
    public Image upLeftImg;
    [Space]
    public Sprite horizontalSprite;
    public Sprite verticalSprite;
    public Sprite downRightSprite;
    public Sprite downLeftSprite;
    public Sprite upRightSprite;
    public Sprite upLeftSprite;
    [Space]
    public TMP_Text horizontalAmmount;
    public TMP_Text verticalAmmount;
    public TMP_Text downRightAmmount;
    public TMP_Text downLeftAmmount;
    public TMP_Text upRightAmmount;
    public TMP_Text upLeftAmmount;
    [Space]
    public Sprite normalSprite;
    public Sprite disabledSprite;
    public Sprite startPointSprite;
    public Sprite endPointSprite;

    public void AssignController()
    {
        for (int y = 0; y < ovMinigame.gridStartPos.GridSize.y; y++)
        {
            for (int x = 0; x < ovMinigame.gridStartPos.GridSize.x; x++)
            {
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().controller = this;
            }
        }
    }

    public void InitializeOverload()
    {
        for (int y = 0; y < ovMinigame.gridStartPos.GridSize.y; y++)
        {
            for (int x = 0; x < ovMinigame.gridStartPos.GridSize.x; x++)
            {
                minigameGrid[y].row[x].GetComponent<Button>().GetComponent<Image>().sprite = normalSprite;
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().direction = OverloadCellCheck.Direction.None;
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isActive = false;
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isDisabled = false;
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isStartCell = false;
                minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isEndCell = false;
            }
        }

        Vector2Int size = ovMinigame.gridBlockades.GridSize;

        int sizeInt = ovMinigame.gridBlockades.GridSize.x;
        var cellsS = ovMinigame.gridStartPos.GetCells();
        var cellsX = ovMinigame.gridEndPos.GetCells();
        var cellsB = ovMinigame.gridBlockades.GetCells();

        layout.constraintCount = sizeInt;

        OverloadAssign();

        switch(sizeInt)
        {
            case 8:
                for (int y = 0; y < ovMinigame.gridStartPos.GridSize.y; y++)
                {
                    for (int x = 0; x < ovMinigame.gridStartPos.GridSize.x; x++)
                    {
                        if (cellsS[y, x])
                        {
                            minigameGrid[y].row[x].GetComponent<Button>().GetComponent<Image>().sprite = startPointSprite;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isActive = true;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isStartCell = true;
                        }
                    }
                }

                for (int y = 0; y < ovMinigame.gridEndPos.GridSize.y; y++)
                {
                    for (int x = 0; x < ovMinigame.gridEndPos.GridSize.x; x++)
                    {
                        if (cellsX[y, x])
                        {
                            minigameGrid[y].row[x].GetComponent<Button>().GetComponent<Image>().sprite = endPointSprite;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isEndCell = true;

                            endPos = minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().cellIndex;
                        }
                    }
                }

                for (int y = 0; y < ovMinigame.gridBlockades.GridSize.y; y++)
                {
                    for (int x = 0; x < ovMinigame.gridBlockades.GridSize.x; x++)
                    {
                        if (cellsB[y, x])
                        {
                            minigameGrid[y].row[x].GetComponent<Button>().GetComponent<Image>().sprite = disabledSprite;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isDisabled = true;
                        }
                    }
                }
                break;
        }
    }

    public void OverloadAssign()
    {
        horizontalImg.sprite = horizontalSprite;
        verticalImg.sprite = verticalSprite;
        downRightImg.sprite = downRightSprite;
        downLeftImg.sprite = downLeftSprite;
        upRightImg.sprite = upRightSprite;
        upLeftImg.sprite = upLeftSprite;

        UpdateValues();
    }

    public void UpdateValues()
    {
        horizontalAmmount.text = ovMinigame.horizontalPieces.ToString();
        verticalAmmount.text = ovMinigame.verticalPieces.ToString();
        downRightAmmount.text = ovMinigame.downRightPieces.ToString();
        downLeftAmmount.text = ovMinigame.downLeftPieces.ToString();
        upRightAmmount.text = ovMinigame.upRightPieces.ToString();
        upLeftAmmount.text = ovMinigame.upLeftPieces.ToString();
    }

    public void OverloadCopyVariables(OverloadCellCheck cellCheck)
    {
        hand.isActive = cellCheck.isActive;

        hand.direction = cellCheck.direction;

        hand.sprite = cellCheck.sprite;
    }

    public void OverloadPasteVariables(OverloadCellCheck cellCheck)
    {
        if(!cellCheck.isEndCell && !cellCheck.isStartCell)
        {
            switch (hand.direction)
            {
                case OverloadCellCheck.Direction.Horizontal:
                    if (ovMinigame.horizontalPieces > 0)
                    {
                        ovMinigame.horizontalPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;

                case OverloadCellCheck.Direction.Vertical:
                    if (ovMinigame.verticalPieces > 0)
                    {
                        ovMinigame.verticalPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;

                case OverloadCellCheck.Direction.UpLeft:
                    if (ovMinigame.upLeftPieces > 0)
                    {
                        ovMinigame.upLeftPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;

                case OverloadCellCheck.Direction.UpRight:
                    if (ovMinigame.upRightPieces > 0)
                    {
                        ovMinigame.upRightPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;

                case OverloadCellCheck.Direction.DownLeft:
                    if (ovMinigame.downLeftPieces > 0)
                    {
                        ovMinigame.downLeftPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;

                case OverloadCellCheck.Direction.DownRight:
                    if (ovMinigame.downRightPieces > 0)
                    {
                        ovMinigame.downRightPieces--;

                        ChangeVariables(cellCheck);
                    }
                    break;
            }
        }

        UpdateValues();
    }

    public void ChangeVariables(OverloadCellCheck cellCheck)
    {
        if (!cellCheck.isActive && !cellCheck.isDisabled)
        {
            cellCheck.isActive = hand.isActive;

            cellCheck.direction = hand.direction;

            cellCheck.sprite = hand.sprite;
            cellCheck.GetComponent<Image>().sprite = cellCheck.sprite;

            CheckNeighbours(cellCheck);
        }
    }

    public void CheckNeighbours(OverloadCellCheck cellCheck)
    {
        Vector2Int indexes = cellCheck.cellIndex;

        if ((indexes.x > 0 && indexes.x < 7) && (indexes.y > 0 && indexes.y < 7))
        {
            OverloadCellCheck upCell = minigameGrid[indexes.y - 1].row[indexes.x].GetComponent<OverloadCellCheck>();
            OverloadCellCheck downCell = minigameGrid[indexes.y + 1].row[indexes.x].GetComponent<OverloadCellCheck>();
            OverloadCellCheck leftCell = minigameGrid[indexes.y].row[indexes.x - 1].GetComponent<OverloadCellCheck>();
            OverloadCellCheck rightCell = minigameGrid[indexes.y].row[indexes.x + 1].GetComponent<OverloadCellCheck>();

            if (upCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.UpLeft || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Up Cell is Active");

                CheckDir(cellCheck, upCell);
            }
            else if(downCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.DownRight))
            {
                print("Down Cell is Active");

                CheckDir(cellCheck, downCell);
            }
            else if(leftCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.UpLeft))
            {
                print("Left Cell is Active");

                CheckDir(cellCheck, leftCell);
            }
            else if(rightCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownRight || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Right Cell is Active");

                CheckDir(cellCheck, rightCell);
            }
        }
        else if(indexes.x == 0 && indexes.y < 7)
        {
            OverloadCellCheck upCell = minigameGrid[indexes.y - 1].row[indexes.x].GetComponent<OverloadCellCheck>();
            OverloadCellCheck downCell = minigameGrid[indexes.y + 1].row[indexes.x].GetComponent<OverloadCellCheck>();

            if (upCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.UpLeft || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Up Cell is Active");

                CheckDir(cellCheck, upCell);
            }
            else if (downCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.DownRight))
            {
                print("Down Cell is Active");

                CheckDir(cellCheck, downCell);
            }
        }
        else if(indexes.x < 7 && indexes.y == 0)
        {
            OverloadCellCheck leftCell = minigameGrid[indexes.y].row[indexes.x - 1].GetComponent<OverloadCellCheck>();
            OverloadCellCheck rightCell = minigameGrid[indexes.y].row[indexes.x + 1].GetComponent<OverloadCellCheck>();

            if (leftCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.UpLeft))
            {
                print("Left Cell is Active");

                CheckDir(cellCheck, leftCell);
            }
            else if (rightCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownRight || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Right Cell is Active");

                CheckDir(cellCheck, rightCell);
            }
        }
        else if(indexes.x == 7 && indexes.y < 7)
        {
            OverloadCellCheck upCell = minigameGrid[indexes.y - 1].row[indexes.x].GetComponent<OverloadCellCheck>();
            OverloadCellCheck downCell = minigameGrid[indexes.y + 1].row[indexes.x].GetComponent<OverloadCellCheck>();

            if (upCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.UpLeft || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Up Cell is Active");

                CheckDir(cellCheck, upCell);
            }
            else if (downCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Vertical || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.DownRight))
            {
                print("Down Cell is Active");

                CheckDir(cellCheck, downCell);
            }
        }
        else if(indexes.x < 7 && indexes.y == 7)
        {
            OverloadCellCheck leftCell = minigameGrid[indexes.y].row[indexes.x - 1].GetComponent<OverloadCellCheck>();
            OverloadCellCheck rightCell = minigameGrid[indexes.y].row[indexes.x + 1].GetComponent<OverloadCellCheck>();

            if (leftCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownLeft || cellCheck.direction == OverloadCellCheck.Direction.UpLeft))
            {
                print("Left Cell is Active");

                CheckDir(cellCheck, leftCell);
            }
            else if (rightCell.isActive && (cellCheck.direction == OverloadCellCheck.Direction.Horizontal || cellCheck.direction == OverloadCellCheck.Direction.DownRight || cellCheck.direction == OverloadCellCheck.Direction.UpRight))
            {
                print("Right Cell is Active");

                CheckDir(cellCheck, rightCell);
            }
        }
    }

    public void CheckDir(OverloadCellCheck cellCheck, OverloadCellCheck neighbour)
    {
        if (neighbour.direction == OverloadCellCheck.Direction.None && !neighbour.isDisabled)
        {
            cellCheck.isActive = true;
        }

        if (neighbour.direction == OverloadCellCheck.Direction.Horizontal)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Horizontal)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownLeft)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownRight)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.UpRight)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.UpLeft)
            {
                cellCheck.isActive = true;
            }
        } else if (neighbour.direction == OverloadCellCheck.Direction.Vertical)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Vertical)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownLeft)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownRight)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.UpRight)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.UpLeft)
            {
                cellCheck.isActive = true;
            }
        } else if (neighbour.direction == OverloadCellCheck.Direction.UpRight)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Horizontal)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.Vertical)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.UpLeft)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownLeft)
            {
                cellCheck.isActive = true;
            } else if (cellCheck.direction == OverloadCellCheck.Direction.DownRight)
            {
                cellCheck.isActive = true;
            }
        } else if (neighbour.direction == OverloadCellCheck.Direction.UpLeft)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Horizontal)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.Vertical)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.UpRight)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.DownLeft)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.DownRight)
            {
                cellCheck.isActive = true;
            }
        } else if (neighbour.direction == OverloadCellCheck.Direction.DownRight)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Horizontal)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.Vertical)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.UpRight)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.DownLeft)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.UpLeft)
            {
                cellCheck.isActive = true;
            }
        } else if (neighbour.direction == OverloadCellCheck.Direction.DownLeft)
        {
            if (cellCheck.direction == OverloadCellCheck.Direction.Horizontal)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.Vertical)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.UpRight)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.UpLeft)
            {
                cellCheck.isActive = true;
            }
            else if (cellCheck.direction == OverloadCellCheck.Direction.DownRight)
            {
                cellCheck.isActive = true;
            }
        }
    }

    public void InitialSetup()
    {
        InitializeOverload();

        ovUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }
}
