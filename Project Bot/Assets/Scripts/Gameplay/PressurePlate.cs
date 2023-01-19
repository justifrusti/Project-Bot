using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public CinemachineVirtualCamera cinematicCam;
    [Space]
    public float adjustSpeed;
    [Space]
    public Transform moveTowards;
    [Space]
    public Vector3 originalPos;
    [Space]
    public bool switchCamOnDeactivate;
    [Space]
    public Material buttonMat;
    [Space]
    public bool playCinematic;
    [Space]
    public SwitchComponent component;
    public SwitchComponent secondComponent = null;
    public SwitchManager componentManager;

    [HideInInspector] public bool onPressureplate;
    
    private bool pressureplateActive = false;

    private void Awake()
    {
        originalPos = transform.position;
        
        if(gameObject.GetComponent<SwitchComponent>() != null)
        {
            component = GetComponent<SwitchComponent>();
        }else if(gameObject.GetComponent<SwitchManager>() != null)
        {
            componentManager = GetComponent<SwitchManager>();
        }
    }

    private void Update()
    {
        if(!onPressureplate)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, adjustSpeed * Time.deltaTime);
        }else if(onPressureplate)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowards.position, adjustSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ActivatePlate") && !pressureplateActive)
        {
            buttonMat.EnableKeyword("_EMISSION");

            pressureplateActive = true;

            if(playCinematic)
            {
                CameraSwitcher.Register(cinematicCam);

                if (CameraSwitcher.IsActiveCamera(cinematicCam))
                {
                    Debug.Log("Cinematic is already active");
                }
                else
                {
                    CameraSwitcher.SwitchCamera(cinematicCam);
                    FindObjectOfType<AudioManagerScript>().Play("PuzzleSolved");
                }
            }

            StartCoroutine(ActivatePressureFunction());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ActivatePlate") && pressureplateActive)
        {
            buttonMat.DisableKeyword("_EMISSION");

            pressureplateActive = false;

            if(switchCamOnDeactivate)
            {
                if (CameraSwitcher.IsActiveCamera(cinematicCam))
                {
                    Debug.Log("Cinematic is already active");
                }
                else
                {
                    CameraSwitcher.SwitchCamera(cinematicCam);
                }
            }

            StartCoroutine(DeactivatePressureFunction());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onPressureplate = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        onPressureplate = false;
    }

    IEnumerator ActivatePressureFunction()
    {
        yield return new WaitForSeconds(1.5f);
        
        if(component != null)
        {
            SwitchComponentState();
        }
        else if(componentManager != null)
        {
            componentManager.switchActive = true;   
        }

        yield return new WaitForSeconds(1.5f);

        if(playCinematic)
        {
            CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
        }
    }

    IEnumerator DeactivatePressureFunction()
    {
        if(playCinematic)
        {
            if (switchCamOnDeactivate)
            {
                yield return new WaitForSeconds(1.5f);

                SwitchComponentState();

                yield return new WaitForSeconds(1.5f);

                CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
            }else
            {
                SwitchComponentState();
            }

            CameraSwitcher.Unregister(cinematicCam);
        }else
        {
            SwitchComponentState();

            CameraSwitcher.Unregister(cinematicCam);
        }
    }

    public void SwitchComponentState()
    {
        if (component.currentAction == SwitchComponent.Action.Disable)
        {
            component.currentAction = SwitchComponent.Action.Enable;
        }
        else if (component.currentAction == SwitchComponent.Action.Enable)
        {
            component.currentAction = SwitchComponent.Action.Disable;
        }

        if (secondComponent != null)
        {
            if (secondComponent.currentAction == SwitchComponent.Action.Disable)
            {
                secondComponent.currentAction = SwitchComponent.Action.Enable;
            }
            else if (secondComponent.currentAction == SwitchComponent.Action.Enable)
            {
                secondComponent.currentAction = SwitchComponent.Action.Disable;
            }
        }
    }
}
