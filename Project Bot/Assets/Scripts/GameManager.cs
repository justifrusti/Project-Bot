using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public SaveData saveData;
    public ThirdPersonPlayerController playerController;
    public PlayerUIManager uiManager;

    public static string directory = "/Data/";
    public static string fileName = "PlayerOS.bot";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeScripts();

        string fullPath = Application.persistentDataPath + directory + fileName;

        if (File.Exists(fullPath))
        {
            LoadGame();
        }
        else
        {
            Debug.LogError("Save file does not exist");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SavePlayerData();
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
            LoadPlayerData();
        }
    }

    public void InitializeScripts()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI Manager").GetComponent<PlayerUIManager>();
    }

    public void SavePlayerData()
    {
        saveData.maxHearts = playerController.maxHearts;
        saveData.maxDamageCharge = playerController.maxDamageCharge;
        saveData.damageChargeSpeed = playerController.damageChargeSpeed;
        saveData.invisFramesTime = playerController.invisFramesTime;
        saveData.chargeShootSpeed = playerController.chargeShootSpeed;
        saveData.invisFramesActive = playerController.invisFramesActive;
        saveData.hearts = playerController.hearts;
        saveData.damage = playerController.damage;
        saveData.walkingSpeed = playerController.walkingSpeed;
        saveData.runSpeed = playerController.runSpeed;
        saveData.speedPadSpeed = playerController.speedPadSpeed;
        saveData.maxJumps = playerController.maxJumps;
        saveData.normalTurnSensitivity = playerController.normalTurnSensitivity;
        saveData.lockedTurnSensitivity = playerController.lockedTurnSensitivity;
        saveData.runTurnSensitivity = playerController.runTurnSensitivity;
        saveData.maxJumpCharge = playerController.maxJumpCharge;
        saveData.jumpChargeSpeed = playerController.jumpChargeSpeed;
        saveData.jump = playerController.jump;
        saveData.jumpPadJump = playerController.jumpPadJump;
        saveData.originalJump = playerController.originalJump;
        saveData.originalWalkSpeed = playerController.originalWalkSpeed;
        saveData.originalRunSpeed = playerController.originalRunSpeed;
        saveData.turnSensitivity = playerController.turnSensitivity;
        saveData.camSens = playerController.camSens;
        saveData.turnMode = playerController.turnMode;
        saveData.pickUpRayDst = playerController.pickUpRayDst;
        saveData.throwForce = playerController.throwForce;
        saveData.dropForce = playerController.dropForce;
        saveData.currentActiveCheckpoint = playerController.currentActiveCheckpoint;

        saveData.skillPoints = SkillTreeReader.Instance.availablePoints;
    }

    public void LoadPlayerData()
    {
        playerController.maxHearts = saveData.maxHearts;
        playerController.maxDamageCharge = saveData.maxDamageCharge;
        playerController.damageChargeSpeed = saveData.damageChargeSpeed;
        playerController.invisFramesTime = saveData.invisFramesTime;
        playerController.chargeShootSpeed = saveData.chargeShootSpeed;
        playerController.invisFramesActive = saveData.invisFramesActive;
        playerController.hearts = saveData.hearts;
        playerController.damage = saveData.damage;
        playerController.walkingSpeed = saveData.walkingSpeed;
        playerController.runSpeed = saveData.runSpeed;
        playerController.speedPadSpeed = saveData.speedPadSpeed;
        playerController.maxJumps = saveData.maxJumps;
        playerController.normalTurnSensitivity = saveData.normalTurnSensitivity;
        playerController.lockedTurnSensitivity = saveData.lockedTurnSensitivity;
        playerController.runTurnSensitivity = saveData.runTurnSensitivity;
        playerController.maxJumpCharge = saveData.maxJumpCharge;
        playerController.jumpChargeSpeed = saveData.jumpChargeSpeed;
        playerController.jump = saveData.jump;
        playerController.jumpPadJump = saveData.jumpPadJump;
        playerController.originalJump = saveData.originalJump;
        playerController.originalWalkSpeed = saveData.originalWalkSpeed;
        playerController.originalRunSpeed = saveData.originalRunSpeed;
        playerController.turnSensitivity = saveData.turnSensitivity;
        playerController.camSens = saveData.camSens;
        playerController.turnMode = saveData.turnMode;
        playerController.pickUpRayDst = saveData.pickUpRayDst;
        playerController.throwForce = saveData.throwForce;
        playerController.dropForce = saveData.dropForce;
        playerController.currentActiveCheckpoint = saveData.currentActiveCheckpoint;

        playerController.RespawnAtCheckpoint();

        SkillTreeReader.Instance.availablePoints = saveData.skillPoints;
    }

    public void SaveGame()
    {
        print("save");
        SaveManager.Save(saveData);
    }

    public void LoadGame()
    {
        print("load");
        saveData = SaveManager.Load();
    }
}
