using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class StageData // 한 판에 대한 내용
{
    public int level;
    public int score;

}

[Serializable]
public class GameData
{    
    public int highestLevel;

    public int achievementCoin;

    public int partIndex_Lower;
    public int partIndex_Upper;
    public int partIndex_LeftArm;
    public int partIndex_RightArm;
    public int partIndex_LeftShoulder;
    public int partIndex_RightShoulder;

    public List<int> unlockedPartsList = new List<int>();
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

    #region Parts Index
    public int PartIndex_Lower { 
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
    #region Rewards
    public int AchievementCoin
    {
        get => gameData.achievementCoin;
        set
        {
            gameData.achievementCoin = value;
            // To Do - UI든 어디든 일단 구독된 친구들에게 이벤트 Invoke
            SaveGame();
        }
    }

    public List<int> UnlockedPartsIDList => gameData.unlockedPartsList;
    public void ReceivePartID(int id)
    {
        gameData.unlockedPartsList.Add(id);
        OnReceivePartID?.Invoke(id);
        SaveGame();
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
