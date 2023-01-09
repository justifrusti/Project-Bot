using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MARController : MonoBehaviour
{
    public enum MARTypes
    {
        Missle,
        Shield
    }

    public MARTypes type;

    [Header("Lists")]
    public List<MARController> turretMARInArea;
    public List<EnemyAI> roombasInArea;
    public List<EnemyAI> turretsInArea;
    public List<MARController> shieldMAR;
    [Space]
    public List<GameObject> protectedEnemiesList;

    [Header("MAR Stats")]
    public int health;
    public int damage;
    [Space]
    public float range;
    [Space]
    [Range(0, 5)]public int enemiesToProtect;

    [Header("MAR Movement")]
    public float speed;

    [Header("Re-Check")]
    public float maxCheckTime;

    [HideInInspector] public int protectedEnemies;
    [HideInInspector] public float checkTime;

    private void Update()
    {
        int totalEnemiesInRange = (turretMARInArea.Count + roombasInArea.Count + turretsInArea.Count + shieldMAR.Count);

        if (checkTime > 0)
        {
            checkTime -= 1 * Time.deltaTime;
        }else if(checkTime <= 0)
        {
            checkTime = maxCheckTime;

            ReCheckInRange();
        }else
        {
            checkTime = maxCheckTime;
        }

        if(protectedEnemies < enemiesToProtect && totalEnemiesInRange >= enemiesToProtect)
        {
            ProtectEnemies(totalEnemiesInRange);
        }else if(totalEnemiesInRange < enemiesToProtect)
        {
            ProtectEnemies(enemiesToProtect);
        }
    }

    public void ReCheckInRange()
    {
        Debug.Log("Reitterate");

        foreach (var enemies in Physics.OverlapSphere(transform.position, range))
        {
            if (enemies.CompareTag("TurretMAR"))
            {
                if (enemies.GetComponent<MARController>() != null)
                {
                    if (!turretMARInArea.Contains(enemies.GetComponent<MARController>()))
                    {
                        turretMARInArea.Add(enemies.GetComponent<MARController>());
                    }
                }
            }
        }

        RemoveEmptySlots();
    }

    public void RemoveEmptySlots()
    {
        for (int i = turretMARInArea.Count - 1; i >= 0; i--)
        {
            float dst = Vector3.Distance(turretMARInArea[i].transform.position, transform.position);

            if (turretMARInArea[i] == null || dst > range)
            {
                turretMARInArea.RemoveAt(i);
            }
        }

        for (int i = roombasInArea.Count - 1; i >= 0; i--)
        {
            float dst = Vector3.Distance(turretMARInArea[i].transform.position, transform.position);

            if (roombasInArea[i] == null || dst > range)
            {
                roombasInArea.RemoveAt(i);
            }
        }

        for (int i = turretsInArea.Count - 1; i >= 0; i--)
        {
            float dst = Vector3.Distance(turretMARInArea[i].transform.position, transform.position);

            if (turretsInArea[i] == null || dst > range)
            {
                turretsInArea.RemoveAt(i);
            }
        }

        for (int i = shieldMAR.Count - 1; i >= 0; i--)
        {
            float dst = Vector3.Distance(turretMARInArea[i].transform.position, transform.position);

            if (shieldMAR[i] == null || dst > range)
            {
                shieldMAR.RemoveAt(i);
            }
        }
    }

    public void ProtectEnemies(int totalEnemiesInRange)
    {
        if (totalEnemiesInRange >= enemiesToProtect)
        {
            //Make Function to calculate which list(s) to use depending on the ammount of enemies present;
        }
    }
}
