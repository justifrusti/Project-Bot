using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

public class PlayerUIManager : MonoBehaviour
{
    public ThirdPersonPlayerController playerController;
    [Space]
    public TMP_Text availableSkillPoints;
    [Space]
    public Image[] availableHearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    [Space]
    public Image blueIndicator;
    public Image yellowIndicator;
    public Image greenIndicator;
    public Image redIndicator;
    [Space]
    public Slider dmgCharger;

    private void Start()
    {
        dmgCharger.maxValue = playerController.maxDamageCharge;
    }

    void Update()
    {
        if(playerController.manager != null && playerController.manager.initialized)
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

            if (Input.GetButton("LMB") && playerController.unlockedShock)
            {
                dmgCharger.gameObject.SetActive(true);

                dmgCharger.value = playerController.damage;
            }else
            {
                dmgCharger.gameObject.SetActive(false);

                dmgCharger.value = 0;
            }
        }
    }

    public void KeyCardIndicators()
    {
        if (playerController.unlockedBlueKK)
        {
            blueIndicator.enabled = true;
        }
        else if (!playerController.unlockedBlueKK)
        {
            blueIndicator.enabled = false;
        }

        if (playerController.unlockedGreeKK)
        {
            greenIndicator.enabled = true;
        }
        else if (!playerController.unlockedGreeKK)
        {
            greenIndicator.enabled = false;
        }

        if (playerController.unlockedYellowKK)
        {
            yellowIndicator.enabled = true;
        }
        else if (!playerController.unlockedYellowKK)
        {
            yellowIndicator.enabled = false;
        }

        if (playerController.unlockedRedKK)
        {
            redIndicator.enabled = true;
        }
        else if (!playerController.unlockedRedKK)
        {
            redIndicator.enabled = false;
        }
    }
}
