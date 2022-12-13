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

    [HideInInspector] public bool onPressureplate;
    private SwitchComponent component;
    [SerializeField]private bool pressureplateActive = false;

    private void Awake()
    {
        originalPos = transform.position;
        component = GetComponent<SwitchComponent>();
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

            CameraSwitcher.Register(cinematicCam);

            if(CameraSwitcher.IsActiveCamera(cinematicCam))
            {
                Debug.Log("Cinematic is already active");
            }else
            {
                CameraSwitcher.SwitchCamera(cinematicCam);
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
        component.currentAction = SwitchComponent.Action.Disable;
        yield return new WaitForSeconds(1.5f);

        CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
    }

    IEnumerator DeactivatePressureFunction()
    {
        yield return new WaitForSeconds(1.5f);
        component.currentAction = SwitchComponent.Action.Enable;
        yield return new WaitForSeconds(1.5f);

        if(switchCamOnDeactivate)
        {
            CameraSwitcher.SwitchPlayerCamera(CameraSwitcher.playerCam);
        }

        CameraSwitcher.Unregister(cinematicCam);
    }
}
