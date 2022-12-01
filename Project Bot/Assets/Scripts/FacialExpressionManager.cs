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

    public FacialColors color;

    public Material materialToChange;

    [Header("Facial Colors")]
    public Color red;
    public Color green;
    public Color blue;
    public Color white;
    public Color amber;
    public Color pink;

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
    }
}
