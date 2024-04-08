using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSaveNLoader : MonoBehaviour
{
    public PerkList tier1PerkData; // 저장되는 Tier1 퍼크 데이터
    public PerkList tier2PerkData; // " Tier2 "
    public PerkList tier3PerkData; // " Tier3 "

    [ContextMenu("To Json Data(Save)")]
    public void SaveData(PerkList perkList, string name)
    {
        string jsonData = JsonUtility.ToJson(perkList, true);
        string path = Path.Combine(Application.dataPath, $"{name}.json");
        File.WriteAllText(path, jsonData);
        Debug.Log($"{name}.json 파일로 저장했음");
    }

    [ContextMenu("From Json Data(Load)")]
    public void LoadData(ref PerkList perkList, string name)
    {
        string path = Path.Combine(Application.dataPath, $"{name}.json");
        if (!File.Exists(path))
        {
            Debug.Log("파일 없음 다시 확인해보셈");
            perkList = new PerkList();
            return;
        }
        string jsonData = File.ReadAllText(path);

        perkList = JsonUtility.FromJson<PerkList>(jsonData);
        Debug.Log($"{name}.json 파일 불러왔음");
    }

    public bool IsExist(string name)
    {
        string path = Path.Combine(Application.dataPath, $"{name}.json");

        if (File.Exists(path))
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class PerkList
{
    public List<PerkInfo> data;
}

[System.Serializable]
public class PerkInfo
{
    public PerkTier Tier;
    public int PositionIdx;
    public int ContentIdx;
    public bool IsActive;

    public PerkInfo(PerkTier tier, int positionIdx, int contentIdx, bool isActive)
    {
        Tier = tier;
        PositionIdx = positionIdx;
        ContentIdx = contentIdx;
        IsActive = isActive;
    }

}

[System.Serializable]
public enum PerkTier
{
    ORIGIN,
    TIER1,
    TIER2,
    TIER3
}
