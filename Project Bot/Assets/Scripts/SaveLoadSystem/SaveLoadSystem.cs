using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public SaveData saveData;

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