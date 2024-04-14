using UnityEngine;

[System.Serializable]
public class LevelData : IEntity
{
    [SerializeField] private int dev_ID;
    [SerializeField] private int stageTime;
    [SerializeField] private int spawnCount;
    [SerializeField] UnitType unitType;

    public int Dev_ID => dev_ID;
    public int SpawnTime => stageTime;
    public int SpawnCount => spawnCount;
    public UnitType UnitType => unitType;
}
