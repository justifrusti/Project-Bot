/*
This Script is written for a Cinemachine Free Look Cam,
by changing the Cam to a different Cinemachine Cam you can change the types of cams this script is used for.
 */

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraShaker
{
    private static CinemachineFreeLook cam;
    static List<CinemachineVirtualCamera> rigs = new List<CinemachineVirtualCamera>();

    public static void RegisterCam(CinemachineFreeLook camToUse)
    {
        if(cam == null)
        {
            cam = camToUse;

            Debug.Log(camToUse.name + " Registered Sucessfully");
        }else
        {
            Debug.Log(camToUse + " Already Registered, unregister cam to register a new cam");
        }
    }

    public static void UnregisterCam()
    {
        cam = null;

        Debug.Log("Unregistered " + cam + "Sucessfully");
    }

    //Initialize List when Starting Shake
    public static void InitializeRig()
    {
        for (int i = 0; i < 3; i++)
        {
            AddRigs(i, rigs);

            if(i == 3)
            {
                break;
            }
        }
    }

    //De Initialize List when Shake Finished
    public static void DeInitializeRigs()
    {
        rigs.Clear();
        Debug.Log("Rigs Cleared Succesfully");
    }

    //Call to activate Shake
    public static void InitializeShake(float shakeIntensity = 5f, float shakeTiming = .5f, float gainage = .5f)
    {
        NonMonobehaviourReferencer.instance.CoroutineTrigger(ProcessShake(shakeIntensity, shakeTiming, gainage));
    }

    //Apply the shake with a certain Intensity for a certain Time
    public static IEnumerator ProcessShake(float shakeIntensity, float shakeTiming, float gainage)
    {
        if(rigs.Count > 0)
        {
            Noise(gainage, shakeIntensity);
            yield return new WaitForSeconds(shakeTiming);
            Noise(0, 0);
            UnregisterCam();
        }else
        {
            InitializeRig();
        }

        Debug.Log("Sucessfully Processed Shake");
    }

    //The Shake Function (Amplitude and FrequencyGain are = Shake Strength)
    public static void Noise(float amplitudeGain, float frequencyGain)
    {
        rigs[0].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        rigs[1].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        rigs[2].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;

        rigs[0].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
        rigs[1].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
        rigs[2].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }

    //Add Rigs to a list so their value's can be altered to create a shake
    public static void AddRigs(int index, List<CinemachineVirtualCamera> listToUse)
    {
        if(!listToUse.Contains(cam.GetRig(index)))
        {
            listToUse.Add(cam.GetRig(index));

            Debug.Log("Sucessfully Added Rig to List");
        }else
        {
            Debug.Log("Rig Already Added, Clear Rigs to refresh List");
        }
    }
}
