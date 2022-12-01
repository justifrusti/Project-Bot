using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FacialExpressionManager : MonoBehaviour
{
    public enum FacialColors
    {
        Red,
        Green,
        Blue,
        White,
        Amber,
        Pink
    }

    public enum CurrentExpression
    {
        Default,
        BlankBlack,
        BlankWhite,
        Death1,
        Death2,
        Death3,
        Happy,
        OverJoyed,
        Pain1,
        Pain2,
        Pain3,
        Shocked1,
        Shocked2,
        Smile,
        Smug,
        Wink
    }

    public FacialColors color;
    public CurrentExpression em;
    public CurrentExpression previousEM;

    public Material materialToChange;

    [Header("Facial Colors")]
    public Color red;
    public Color green;
    public Color blue;
    public Color white;
    public Color amber;
    public Color pink;

    [Header("Facial EM")]
    public Texture em_Default;
    public Texture em_blankBlack;
    public Texture em_blankWhite;
    public Texture em_Death1;
    public Texture em_Death2;
    public Texture em_Death3;
    public Texture em_Happy;
    public Texture em_OverJoyed;
    public Texture em_Pain1;
    public Texture em_Pain2;
    public Texture em_Pain3;
    public Texture em_Shocked1;
    public Texture em_Shocked2;
    public Texture em_Smile;
    public Texture em_Smug;
    public Texture em_Wink;

    private void Update()
    {
        switch(color)
        {
            case FacialColors.Red:
                materialToChange.SetColor("_BaseColor", red);
                materialToChange.SetColor("_EmissionColor", red);
                break;

            case FacialColors.Green:
                materialToChange.SetColor("_BaseColor", green);
                materialToChange.SetColor("_EmissionColor", green);
                break;

            case FacialColors.Blue:
                materialToChange.SetColor("_BaseColor", blue);
                materialToChange.SetColor("_EmissionColor", blue);
                break;

            case FacialColors.White:
                materialToChange.SetColor("_BaseColor", white);
                materialToChange.SetColor("_EmissionColor", white);
                break;

            case FacialColors.Amber:
                materialToChange.SetColor("_BaseColor", amber);
                materialToChange.SetColor("_EmissionColor", amber);
                break;

            case FacialColors.Pink:
                materialToChange.SetColor("_BaseColor", pink);
                materialToChange.SetColor("_EmissionColor", pink);
                break;
        }

        if(previousEM != em)
        {
            ChangeEM();
        }
    }

    public void ChangeEM()
    {
        switch(em)
        {
            case CurrentExpression.Default:
                materialToChange.SetTexture("_BaseMap", em_Default);
                materialToChange.SetTexture("_EmissionMap", em_Default);
                break;

            case CurrentExpression.BlankBlack:
                materialToChange.SetTexture("_BaseMap", em_blankBlack);
                materialToChange.SetTexture("_EmissionMap", em_blankBlack);
                break;

            case CurrentExpression.BlankWhite:
                materialToChange.SetTexture("_BaseMap", em_blankWhite);
                materialToChange.SetTexture("_EmissionMap", em_blankWhite);
                break;

            case CurrentExpression.Death1:
                materialToChange.SetTexture("_BaseMap", em_Death1);
                materialToChange.SetTexture("_EmissionMap", em_Death1);
                break;

            case CurrentExpression.Death2:
                materialToChange.SetTexture("_BaseMap", em_Death2);
                materialToChange.SetTexture("_EmissionMap", em_Death2);
                break;

            case CurrentExpression.Death3:
                materialToChange.SetTexture("_BaseMap", em_Death3);
                materialToChange.SetTexture("_EmissionMap", em_Death3);
                break;

            case CurrentExpression.Happy:
                materialToChange.SetTexture("_BaseMap", em_Happy);
                materialToChange.SetTexture("_EmissionMap", em_Happy);
                break;

            case CurrentExpression.OverJoyed:
                materialToChange.SetTexture("_BaseMap", em_OverJoyed);
                materialToChange.SetTexture("_EmissionMap", em_OverJoyed);
                break;

            case CurrentExpression.Pain1:
                materialToChange.SetTexture("_BaseMap", em_Pain1);
                materialToChange.SetTexture("_EmissionMap", em_Pain1);
                break;

            case CurrentExpression.Pain2:
                materialToChange.SetTexture("_BaseMap", em_Pain2);
                materialToChange.SetTexture("_EmissionMap", em_Pain2);
                break;

            case CurrentExpression.Pain3:
                materialToChange.SetTexture("_BaseMap", em_Pain3);
                materialToChange.SetTexture("_EmissionMap", em_Pain3);
                break;

            case CurrentExpression.Shocked1:
                materialToChange.SetTexture("_BaseMap", em_Shocked1);
                materialToChange.SetTexture("_EmissionMap", em_Shocked1);
                break;

            case CurrentExpression.Shocked2:
                materialToChange.SetTexture("_BaseMap", em_Shocked2);
                materialToChange.SetTexture("_EmissionMap", em_Shocked2);
                break;

            case CurrentExpression.Smile:
                materialToChange.SetTexture("_BaseMap", em_Smile);
                materialToChange.SetTexture("_EmissionMap", em_Smile);
                break;

            case CurrentExpression.Smug:
                materialToChange.SetTexture("_BaseMap", em_Smug);
                materialToChange.SetTexture("_EmissionMap", em_Smug);
                break;

            case CurrentExpression.Wink:
                materialToChange.SetTexture("_BaseMap", em_Wink);
                materialToChange.SetTexture("_EmissionMap", em_Wink);
                break;
        }

        previousEM = em;
    }
}
