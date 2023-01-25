using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class SwitchManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinematicCam;

    public SwitchComponent[] components;

    public bool switchActive, initializedComponents;

    public GameObject timer;

    public TMP_Text timeText;

    public float maxTime;
    public float timeLeft;

    public bool playCinematic, playCinematicOnDeactive;

    private void Update()
    {
        if (switchActive && !initializedComponents)
        {
            timeLeft = maxTime;

            InitializeComponents();

            if(playCinematic)
            {
                CinematicCutscene();
            }

            timer.SetActive(true);

            initializedComponents = true;

            StartCoroutine(Beep(Mathf.RoundToInt(maxTime)));
        }

        if (switchActive && initializedComponents)
        {
            timeLeft -= 1 * Time.deltaTime;

            timeText.text = timeLeft.ToString("f2");
        }

        if (timeLeft <= 0)
        {
            switchActive = false;
            initializedComponents = false;

            InitializeComponents();

            if(playCinematicOnDeactive)
            {
                CinematicCutscene();
            }

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
            }else if(components[i].type == SwitchComponent.ComponentType.Platform)
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

    public void CinematicCutscene()
    {
        CameraSwitcher.Register(cinematicCam);

        StartCoroutine(CinematicCutsceneTimer());
    }

    IEnumerator CinematicCutsceneTimer()
    {
        yield return new WaitForSeconds(.5f);
        CameraSwitcher.SwitchCamera(cinematicCam);
        yield return new WaitForSeconds(2f);
        CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
        CameraSwitcher.Unregister(cinematicCam);
    }

    IEnumerator Beep(int time)
    {
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(1);

            print("Super Awesome timer beep sound intensifies");
        }
    }
}
