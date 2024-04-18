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

    public ContentList tier1ContentData; // Tier1 콘텐츠 데이터
    public ContentList tier2ContentData; // " Tier2 "
    public ContentList tier3ContentData; // " Tier3 "

    private string _perkDataPath = "Resources/Data/Perk/Saved";
    private string _contentDataPath = "Resources/Data/Perk/Content";

    [ContextMenu("To Json Data(Save/Perk)")]
    public void SavePerkData(PerkList perkList, string name)
    {        
        string jsonData = JsonUtility.ToJson(perkList, true);
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, $"{_perkDataPath}/{name}.json");
#else
        string path = Path.Combine(Application.dataPath, $"{name}.json");
#endif
        File.WriteAllText(path, jsonData);
        Debug.Log($"{name}.json 파일로 저장했음");
    }

    [ContextMenu("From Json Data(Load/Perk)")]
    public void LoadPerkData(ref PerkList perkList, string name)
    {
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, $"{_perkDataPath}/{name}.json");
#else
        string path = Path.Combine(Application.dataPath, $"{name}.json");
#endif
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

    [ContextMenu("To Json Data(Save/Content)")]
    public void SaveContentData(ContentList contentList, string name)
    {
        string jsonData = JsonUtility.ToJson(contentList, true);
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, $"{_contentDataPath}/{name}.json");
#else
        string path = Path.Combine(Application.dataPath, $"{name}.json");
#endif
        File.WriteAllText(path, jsonData);
        Debug.Log($"{name}.json 파일로 저장했음");
    }

    [ContextMenu("From Json Data(Load/Content)")]
    public void LoadContentData(ref ContentList contentList, string name)
    {
        string path = Resources.Load<TextAsset>($"Data/Perk/Content/{name}").text;

        contentList = JsonUtility.FromJson<ContentList>(path);
        Debug.Log($"{name}.json 파일 불러왔음");
    }

    public bool IsPerkDataExist(string name)
    {
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, $"{_perkDataPath}/{name}.json");
#else
        string path = Path.Combine(Application.dataPath, $"{name}.json");
#endif

        if (File.Exists(path))
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class PerkList
{
    public int point;
    public int unlockCount;
    public string currentSeed;
    public List<PerkInfo> data;
}

[System.Serializable]
public class PerkInfo
{
    public PerkTier Tier;
    public int PositionIdx;
    public int ContentIdx;
    public bool IsActive;
    public List<SubPerkInfo> subPerks;

    public PerkInfo(PerkTier tier, int positionIdx, int contentIdx, bool isActive)
    {
        Tier = tier;
        PositionIdx = positionIdx;
        ContentIdx = contentIdx;
        IsActive = isActive;
    }

}

[System.Serializable]
public class SubPerkInfo
{
    public int PositionIdx;
    public int ContentIdx;
    public bool IsActive;

    public SubPerkInfo(int positionIdx, int contentIdx, bool isActive)
    {
        PositionIdx = positionIdx;
        ContentIdx = contentIdx;
        IsActive = isActive;
    }
}

[System.Serializable]
public enum PerkTier
{
    SUB,
    TIER1,
    TIER2,
    TIER3,
    ORIGIN
}

[System.Serializable]
public class ContentList
{
    public PerkTier contentTier;
    public List<ContentInfo> data;
}

[System.Serializable]
public class ContentInfo
{
    public int contentIdx;
    public string name;
    public string description;
    public PerkType type;
    public float value;
}
