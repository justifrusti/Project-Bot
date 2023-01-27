using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(SkillTreeReader))]
public class GameManager : MonoBehaviour
{
    public SaveData saveData;
    public ThirdPersonPlayerController playerController;
    public PlayerUIManager uiManager;
    public FacialExpressionManager facialManager;
    public SkillTreeReader treeReader;

    public AudioSource music;

    public static string directory = "/Data/";
    public static string fileName = "PlayerOS.bot";

    [HideInInspector]public bool initialized = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
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

        treeReader.Initialize();

        if (music != null)
        {
            music.Play();
        }else
        {
            Debug.Log("No Music Present");
        }

        if (playerController != null)
        {
            playerController.Initialize();
        }
        else
        {
            Debug.Log(playerController.GetType().ToString() + " not present");
        }

        if (uiManager != null)
        {
            NoInitialize(uiManager.GetType().ToString());
        }else
        {
            Debug.Log(uiManager.GetType().ToString() + " not present");
        }

        if (facialManager != null) 
        {
            facialManager.ChangeEM(true, 10.0f, FacialExpressionManager.CurrentExpression.Happy);
            StartCoroutine(playerController.ResetBool(10.0f));
            NoInitialize(facialManager.GetType().ToString());
        }else
        {
            Debug.Log(facialManager.GetType().ToString() + " not present");
        }

        initialized = true;
    }

    private void Update()
    {
        if(initialized)
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

            if (Input.GetButtonDown("Respawn") && playerController.hearts <= 0 && playerController.hasDied)
            {
                Destroy(playerController.spawnedDummy);
                Destroy(playerController.particlesSpawnedDummy);

                playerController.gameObject.SetActive(true);

                playerController.gameObject.transform.position = playerController.currentActiveCheckpoint;

                playerController.hasDied = false;

                playerController.hearts = playerController.maxHearts;
            }
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            playerController.maxHearts += 100;
            playerController.hearts = playerController.maxHearts;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            facialManager.color = FacialExpressionManager.FacialColors.White;
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            facialManager.color = FacialExpressionManager.FacialColors.Green;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            facialManager.color = FacialExpressionManager.FacialColors.Red;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            facialManager.color = FacialExpressionManager.FacialColors.Blue;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            facialManager.color = FacialExpressionManager.FacialColors.Amber;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            facialManager.color = FacialExpressionManager.FacialColors.Pink;
        }
    }

    public void InitializeScripts()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerController>();
        uiManager = GameObject.FindGameObjectWithTag("UI Manager").GetComponent<PlayerUIManager>();
        facialManager = GameObject.FindGameObjectWithTag("Player").GetComponent<FacialExpressionManager>();
        treeReader = GetComponent<SkillTreeReader>();
    }

    public void ReInitialize()
    {
        Initialize();
    }

    public void NoInitialize(string scriptName)
    {
        Debug.Log("No Initialize Present In: " + scriptName);
    }

    public void SavePlayerData()
    {
        saveData.maxHearts = playerController.maxHearts;
        saveData.maxDamageCharge = playerController.maxDamageCharge;
        saveData.damageChargeSpeed = playerController.damageChargeSpeed;
        saveData.invisFramesTime = playerController.invisFramesTime;
        saveData.chargeShootSpeed = playerController.chargeShootSpeed;
        saveData.deaths = playerController.deaths;
        saveData.invisFramesActive = playerController.invisFramesActive;
        saveData.hearts = playerController.hearts;
        saveData.damage = playerController.damageToApply;
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
        playerController.deaths = saveData.deaths;
        playerController.invisFramesActive = saveData.invisFramesActive;
        playerController.hearts = saveData.hearts;
        playerController.damageToApply = saveData.damage;
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
