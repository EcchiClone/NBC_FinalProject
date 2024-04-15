using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private Dictionary<int, PartData> _partsDict = new Dictionary<int, PartData>();
    private Dictionary<int, LevelData> _levelDict = new Dictionary<int, LevelData>();

    public void Init()
    {
        LoadAllPartDatas(_partsDict);
        LoadAllLevelDatas(_levelDict);
    }

    private void LoadAllDatas<T1, T2>(Dictionary<int, T2> dict, string fileName = null) where T1 : BaseDbSheet<T2> where T2 : IEntity
    {
        if (string.IsNullOrEmpty(fileName)) // 파일이름이 공란이면 T1의 이름과 같다. (Reimport 할 때 Class & SO 이름)
            fileName = typeof(T1).Name;

        string path = $"Data/{fileName}";

        var dataSheet = Resources.Load<T1>(path); // SO Load
        var dataSO = Object.Instantiate(dataSheet); // SO 복제(참조)
        var entities = dataSO.Entities;

        if (entities == null || entities.Count <= 0)
        {
            Debug.LogWarning($"불러올 데이터가 존재하지 않습니다. DataSheet : {dataSheet.name}");
            return;
        }

        int entityCount = entities.Count;
        for (int i = 0; i < entityCount; ++i)
        {
            var entity = entities[i];

            if (dict.ContainsKey(entity.Dev_ID)) // 있으면 안 되지 않나?
                dict[entity.Dev_ID] = entity;
            else
                dict.Add(entity.Dev_ID, entity);
        }
    }

    private void LoadAllPartDatas(Dictionary<int, PartData> dict) => LoadAllDatas<PartDbSheet, PartData>(dict);
    private void LoadAllLevelDatas(Dictionary<int, LevelData> dict) => LoadAllDatas<LevelDbSheet, LevelData>(dict);

    private T GetData<T>(int id, Dictionary<int, T> dict) where T : IEntity
    {
        if (dict.ContainsKey(id))
            return dict[id];

        return default;
    }

    public PartData GetPartData(int id) => GetData(id, _partsDict);
    public LevelData GetLevelData(int id) => GetData(id, _levelDict);

    public IEnumerator GetEnumerator<T>(Dictionary<int, T> dict) where T : IEntity => dict.GetEnumerator();

    public IEnumerator GetPartDataEnumerator() => GetEnumerator(_partsDict);
}
