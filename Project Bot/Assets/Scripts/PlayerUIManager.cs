using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public ThirdPersonPlayerController playerController;
    [Space]
    public TMP_Text availableSkillPoints;
    [Space]
    public Image[] availableHearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        availableSkillPoints.text = "Available Skill Points: " + SkillTreeReader.Instance.availablePoints.ToString();

        for (int i = 0; i < availableHearts.Length; i++)
        {
            if (i < playerController.hearts)
            {
                availableHearts[i].sprite = fullHeart;
            }
            else
            {
                availableHearts[i].sprite = emptyHeart;
            }

            if (i < playerController.maxHearts)
            {
                availableHearts[i].enabled = true;
            }
            else
            {
                availableHearts[i].enabled = false;
            }
        }
    }
}
