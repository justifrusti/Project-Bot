using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMGChargeSliderManager : MonoBehaviour
{
    public GameManager manager;

    public Slider slider;
    public GameObject sliderObj;

    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.maxValue = manager.playerController.maxDamageCharge;
    }

    void Update()
    {
        if(Input.GetButton("LMB"))
        {
            sliderObj.SetActive(true);

            slider.value = manager.playerController.damageToApply;
        }else
        {
            sliderObj.SetActive(false);

            slider.value = 0;
        }
    }
}
