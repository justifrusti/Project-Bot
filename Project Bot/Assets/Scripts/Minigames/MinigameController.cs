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

    public enum MinigameType
    {
        Overload,
        Download
    }

    public MinigameType minigameType;

    public List<GridRow> minigameGrid;
    public GridLayoutGroup layout;

    [Header("Minigame Ref")]
    public OverloadingMinigame ovMinigame;
    [Space]
    [Header("")]
    public OverloadCellCheck hand;
    [Space]
    [Header("UI Elements")]
    public GameObject ovUI;
    /*public GameObject dlUI;*/
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
    public Sprite disabledSprite;
    public Sprite startPointSprite;
    public Sprite endPointSprite;

    private void Start()
    {
        if(ovMinigame != null)
        {
            minigameType = MinigameType.Overload;
        }else
        {
            minigameType = MinigameType.Download;
        }

        switch(minigameType)
        {
            case MinigameType.Overload:
                /*dlUI.SetActive(false);*/

                InitializeOverload();

                ovUI.SetActive(true);
                break;

            case MinigameType.Download:
                ovUI.SetActive(false);

                //Initialize Stuff

                /*dlUI.SetActive(true);*/
                break;
        }
    }

    public void InitializeOverload()
    {
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
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().horizontal = true;
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
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isActive = true;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().isEndCell = true;
                            minigameGrid[y].row[x].GetComponent<OverloadCellCheck>().horizontal = true;
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
                            minigameGrid[y].row[x].GetComponent<Button>().GetComponentInChildren<Text>().text = "";
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
        hand.horizontal = cellCheck.horizontal;
        hand.vertical = cellCheck.vertical;
        hand.upLeft = cellCheck.upLeft;
        hand.upRight = cellCheck.upRight;
        hand.downLeft = cellCheck.downLeft;
        hand.downRight = cellCheck.downRight;

        hand.sprite = cellCheck.sprite;
    }

    public void OverloadPasteVariables(OverloadCellCheck cellCheck)
    {
        if(!cellCheck.isActive && !cellCheck.isDisabled)
        {
            cellCheck.isActive = hand.isActive;
            cellCheck.horizontal = hand.horizontal;
            cellCheck.vertical = hand.vertical;
            cellCheck.upLeft = hand.upLeft;
            cellCheck.upRight = hand.upRight;
            cellCheck.downLeft = hand.downLeft;
            cellCheck.downRight = hand.downRight;

            cellCheck.sprite = hand.sprite;
            cellCheck.GetComponent<Image>().sprite = cellCheck.sprite;
        }
    }
}
