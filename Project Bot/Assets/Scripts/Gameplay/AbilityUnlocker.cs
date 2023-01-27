using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    public enum Ability
    {
        Shock,
        Hacking
    }

    public Ability abilityType;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(abilityType == Ability.Shock) 
            {
                collision.gameObject.GetComponent<ThirdPersonPlayerController>().unlockedShock = true;
                FindObjectOfType<AudioManagerScript>().Play("ItemGet");

                Destroy(this.gameObject);
            }else if(abilityType == Ability.Hacking)
            {
                collision.gameObject.GetComponent<ThirdPersonPlayerController>().unlockedHacking = true;
                FindObjectOfType<AudioManagerScript>().Play("ItemGet");

                Destroy(this.gameObject);
            }
        }
    }
}
