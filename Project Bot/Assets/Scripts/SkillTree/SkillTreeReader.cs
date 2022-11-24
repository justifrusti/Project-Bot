using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkillTreeReader : MonoBehaviour
{
    private static SkillTreeReader _instance;

    public static SkillTreeReader Instance
    {
        get
        {
            return _instance;
        }set
        {

        }
    }

    private Skill[] _skillTree;

    private Dictionary<int, Skill> _skills;

    private Skill _skillInspected;

    public int availablePoints = 100;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            SetUpSkillTree();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void SetUpSkillTree()
    {
        _skills = new Dictionary<int, Skill>();

        if (PlayerPrefs.GetString("SkillTree", "").CompareTo("") == 0)
        {
            LoadSkillTree();
        }
        else
        {
            LoadPlayerSkillTree();
        }


    }

    public void LoadSkillTree()
    {
        string path = "Assets/Scripts/SkillTree/Data/skilltree.json";
        string dataAsJson;

        if (File.Exists(path))
        {
������������dataAsJson = File.ReadAllText(path);

            SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

            _skillTree = new Skill[loadedData.skillTree.Length];
            _skillTree = loadedData.skillTree;

            for (int i = 0; i < _skillTree.Length; ++i)
            {
                _skills.Add(_skillTree[i].id_Skill, _skillTree[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    public void LoadPlayerSkillTree()
    {
        string dataAsJson = PlayerPrefs.GetString("SkillTree");

        SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

        _skillTree = new Skill[loadedData.skillTree.Length];
        _skillTree = loadedData.skillTree;

        for (int i = 0; i < _skillTree.Length; ++i)
        {
            _skills.Add(_skillTree[i].id_Skill, _skillTree[i]);
        }
    }

    public void SaveSkillTree()
    {
��������SkillTree skillTree = new SkillTree();
        skillTree.skillTree = new Skill[_skillTree.Length];

        for (int i = 0; i < _skillTree.Length; ++i)
        {
            _skills.TryGetValue(_skillTree[i].id_Skill, out _skillInspected);

            if (_skillInspected != null)
            {
                skillTree.skillTree[i] = _skillInspected;
            }
        }

        string json = JsonUtility.ToJson(skillTree);

        PlayerPrefs.SetString("SkillTree", json);
    }

    public bool IsSkillUnlocked(int id_skill)
    {
        if (_skills.TryGetValue(id_skill, out _skillInspected))
        {
            return _skillInspected.unlocked;
        }
        else
        {
            return false;
        }
    }

    public bool CanSkillBeUnlocked(int id_skill)
    {
        bool canUnlock = true;

        if (_skills.TryGetValue(id_skill, out _skillInspected)) // The skill exists
��������{
            if (_skillInspected.cost <= availablePoints) // Enough points available
������������{
                int[] dependencies = _skillInspected.skill_Dependencies;

                for (int i = 0; i < dependencies.Length; ++i)
                {
                    if (_skills.TryGetValue(dependencies[i], out _skillInspected))
                    {
                        if (!_skillInspected.unlocked)
                        {
                            canUnlock = false;
                            break;
                        }
                    }else // If one of the dependencies doesn't exist, the skill can't be unlocked.
��������������������{
                        return false;
                    }
                }
            }else // If the player doesn't have enough skill points, can't unlock the new skill
������������{
                return false;
            }

        }else // If the skill id doesn't exist, the skill can't be unlocked
��������{
            return false;
        }

        return canUnlock;
    }

    public bool UnlockSkill(int id_Skill)
    {
        if (_skills.TryGetValue(id_Skill, out _skillInspected))
        {
            if (_skillInspected.cost <= availablePoints)
            {
                availablePoints -= _skillInspected.cost;
                _skillInspected.unlocked = true;

                _skills.Remove(id_Skill);
                _skills.Add(id_Skill, _skillInspected);

                return true;
            }
            else
            {
                return false;   // The skill can't be unlocked. Not enough points
������������}
        }
        else
        {
            return false;   // The skill doesn't exist
��������}
    }
}