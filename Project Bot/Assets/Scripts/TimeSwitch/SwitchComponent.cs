using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    public enum ComponentType
    {
        Door,
        Platform,
    }

    public enum Action
    {
        Disable,
        Enable
    }

    public ComponentType type;
    public Action currentAction;
    [Space]
    public CinemachineVirtualCamera cinematicCam;
    public GameObject switchObject;
    [Space]
    public Vector3[] path;
    [Space]
    public float originalPlatformMoveSpeed;
    [HideInInspector] public float platformMoveSpeed;
    private int index;
    private bool movingBack = false;
    private bool canMove = true;

    private void Start()
    {
        platformMoveSpeed = originalPlatformMoveSpeed;
    }

    private void Update()
    {
        switch (currentAction)
        {
            case Action.Disable:
                switch (type)
                {
                    case ComponentType.Door:
                        switchObject.SetActive(false);
                        break;
                }
                break;

            case Action.Enable:
                switch (type)
                {
                    case ComponentType.Door:
                        switchObject.SetActive(true);
                        break;

                    case ComponentType.Platform:
                        platformMoveSpeed = originalPlatformMoveSpeed;

                        if (switchObject.transform.position != path[index] && canMove)
                        {
                            switchObject.transform.position = Vector3.MoveTowards(switchObject.transform.position, path[index], platformMoveSpeed * Time.deltaTime);
                        } else if (!movingBack && canMove)
                        {
                            index++;

                            ChangeDir();
                        } else if (movingBack && canMove)
                        {
                            index--;

                            ChangeDir();
                        }

                        break;
                }
                break;
        }
    }

    public void ChangeDir()
    {
        if (index >= path.Length - 1 && !movingBack)
        {
            canMove = false;
            StartCoroutine(MovingBack());
        }
        else if ((index == -1 || index == 0) && movingBack)
        {
            canMove = false;
            StartCoroutine(MovingFront());
        }
    }

    public void CinematicCutscene()
    {
        CameraSwitcher.Register(cinematicCam);

        StartCoroutine(CinematicCutsceneTimer());
    }

    IEnumerator MovingBack()
    {
        movingBack = true;
        yield return new WaitForSeconds(3);
        canMove = true;
    }

    IEnumerator MovingFront()
    {
        movingBack = false;
        yield return new WaitForSeconds(3);
        canMove = true;
    }

    IEnumerator CinematicCutsceneTimer()
    {
        yield return new WaitForSeconds(.5f);
        CameraSwitcher.SwitchCamera(cinematicCam);
        yield return new WaitForSeconds(2f);
        CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
        CameraSwitcher.Unregister(cinematicCam);
    }
}
