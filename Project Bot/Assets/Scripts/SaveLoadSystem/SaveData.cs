using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

[System.Serializable]
public class SaveData
{
    [Header("Health")]
    public int maxHearts;
    [Space]
    public int maxDamageCharge;
    public float damageChargeSpeed;
    [Space]
    public float invisFramesTime;
    public int chargeShootSpeed;
    [Space]
    public int deaths;

    public bool invisFramesActive;
    public int hearts;
    public float damage;

    [Header("Character/Cam Movement")]
    public int walkingSpeed;
    public int runSpeed;
    public int speedPadSpeed;
    public int maxJumps;
    [Space]
    public float normalTurnSensitivity;
    public float lockedTurnSensitivity;
    public float runTurnSensitivity;
    [Space]
    public float maxJumpCharge;
    public float jumpChargeSpeed;
    [Space]
    public Vector3 jump;
    [Space]
    public Vector3 jumpPadJump;
    public Vector3 originalJump;
    [Space]
    public int originalWalkSpeed;
    public int originalRunSpeed;
    [HideInInspector] public float turnSensitivity;
    [HideInInspector] public float camSens;
    public bool turnMode;
    [Space]
    [Header("Pick Ups")]
    public float pickUpRayDst;
    public float throwForce;
    public float dropForce;
    [Space]
    public Vector3 currentActiveCheckpoint;
    [Header("Skills")]
    public int skillPoints;
}
