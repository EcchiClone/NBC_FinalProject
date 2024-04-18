using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PerkData
{
    public float[] perkValueArray = new float[18];

    public void SetActivedPerk(PerkType type, float value, PerkData data)
    {
        perkValueArray[(int)type] += value;
        Managers.GameManager.PerkData = data;
    }

    public float GetAbilityValue(PerkType type)
    {
        return perkValueArray[(int)type];
    }
}


[Serializable]
public class StageData
{
    public float time;
    public int stage;

    public int minionKill;
    public int bossKill;
    public int researchPoint;
}

[Serializable]
public class GameData
{
    public float bestTime;
    public int highestStage;
    public int highestMinionKill;
    public int highestBossKill;
    public int researchPoint;

    public int partIndex_Lower;
    public int partIndex_Upper;
    public int partIndex_LeftArm;
    public int partIndex_RightArm;
    public int partIndex_LeftShoulder;
    public int partIndex_RightShoulder;

    public int achievementPoint = 10000;

    public bool tutorialClear;

    public List<int> unlockedPartsList;
    public PerkData perkData;
    public StageData stageData;
}

public class GameManager
{
    public GameData gameData = new GameData();

    private string _filePath;

    #region Events
    public event Action<int> OnReceivePartID;
    #endregion

    public void Init()
    {
        _filePath = Path.Combine(Application.dataPath, "GameData.json");

        if (File.Exists(_filePath))
            gameData = LoadGame();
    }

    #region StageData
    public StageData StageData
    {
        get
        {
            if(gameData.stageData == null)
                gameData.stageData = new StageData();
            return gameData.stageData;
        }
        set
        {
            gameData.stageData = value;
            SaveGame();
        }
    }
    #endregion
    #region Parts Index
    public int PartIndex_Lower
    {
        get => gameData.partIndex_Lower;
        set
        {
            gameData.partIndex_Lower = value;
            SaveGame();
        }
    }
    public int PartIndex_Upper
    {
        get => gameData.partIndex_Upper;
        set
        {
            gameData.partIndex_Upper = value;
            SaveGame();
        }
    }
    public int PartIndex_LeftArm
    {
        get => gameData.partIndex_LeftArm;
        set
        {
            gameData.partIndex_LeftArm = value;
            SaveGame();
        }
    }
    public int PartIndex_RightArm
    {
        get => gameData.partIndex_RightArm;
        set
        {
            gameData.partIndex_RightArm = value;
            SaveGame();
        }
    }
    public int PartIndex_LeftShoulder
    {
        get => gameData.partIndex_LeftShoulder;
        set
        {
            gameData.partIndex_LeftShoulder = value;
            SaveGame();
        }
    }
    public int PartIndex_RightShoulder
    {
        get => gameData.partIndex_RightShoulder;
        set
        {
            gameData.partIndex_RightShoulder = value;
            SaveGame();
        }
    }
    #endregion
    #region Perk
    public PerkData PerkData
    {
        get
        {
            if (gameData.perkData == null)
                gameData.perkData = new PerkData();
            return gameData.perkData;
        }
        set
        {
            gameData.perkData = value;
            SaveGame();
        }
    }
    #endregion
    #region Rewards
    public int AchievementPoint
    {
        get => gameData.achievementPoint;
        set
        {
            gameData.achievementPoint = value;
            SaveGame();
        }
    }

    public int ResearchPoint
    {
        get => gameData.researchPoint;
        set
        {
            gameData.researchPoint = value;
            SaveGame();
        }
    }

    public List<int> UnlockedPartsIDList
    {
        get
        {
            if (gameData.unlockedPartsList == null)
                gameData.unlockedPartsList = new List<int>();
            return gameData.unlockedPartsList;
        }
    }

    public void ReceivePartID(int id)
    {
        gameData.unlockedPartsList.Add(id);
        OnReceivePartID?.Invoke(id);
        SaveGame();
    }
    #endregion
    #region Tutorial
    public bool TutorialClear
    {
        get => gameData.tutorialClear;
        set
        {
            gameData.tutorialClear = value;
            SaveGame();
        }
    }
    #endregion

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData, true);

        File.WriteAllText(_filePath, json);
        Debug.Log("Game Data Saved");
    }

    public GameData LoadGame()
    {
        string json = File.ReadAllText(_filePath);

        GameData loadedGameData = JsonUtility.FromJson<GameData>(json);
        return loadedGameData;
    }
}
