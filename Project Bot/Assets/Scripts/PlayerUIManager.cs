using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public TMP_Text availableSkillPoints;

    void Update()
    {
        availableSkillPoints.text = "Available Skill Points: " + SkillTreeReader.Instance.availablePoints.ToString();
    }
}
