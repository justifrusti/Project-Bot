using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchManager : MonoBehaviour
{
    public SwitchComponent[] components;

    public bool switchActive, initializedComponents;

    public GameObject timer;

    public TMP_Text timeText;

    public float maxTime;
    public float timeLeft;

    private void Update()
    {
        if(switchActive && !initializedComponents)
        {
            timeLeft = maxTime;

            InitializeComponents();

            timer.SetActive(true);

            initializedComponents = true;
        }

        if(switchActive && initializedComponents)
        {
            timeLeft -= 1 * Time.deltaTime;

            timeText.text = timeLeft.ToString("f2");
        }

        if(timeLeft <= 0)
        {
            switchActive = false;
            initializedComponents = false;

            InitializeComponents();

            timer.SetActive(false);

            timeLeft = maxTime;
        }
    }

    public void InitializeComponents()
    {
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].type == SwitchComponent.ComponentType.Door)
            {
                if (components[i].currentAction == SwitchComponent.Action.Enable)
                {
                    components[i].currentAction = SwitchComponent.Action.Disable;
                }else if(components[i].currentAction == SwitchComponent.Action.Disable)
                {
                    components[i].currentAction = SwitchComponent.Action.Enable;
                }
            }

            if(components[i].type == SwitchComponent.ComponentType.Platform)
            {
                if (components[i].currentAction == SwitchComponent.Action.Enable)
                {
                    components[i].currentAction = SwitchComponent.Action.Disable;
                }
                else if (components[i].currentAction == SwitchComponent.Action.Disable)
                {
                    components[i].currentAction = SwitchComponent.Action.Enable;
                }
            }
        }
    }
}
