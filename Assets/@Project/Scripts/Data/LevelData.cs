using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : IEntity
{
    [SerializeField] private int dev_ID;    
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnDelayTime;
    [SerializeField] private float countDownTime;
    [SerializeField] UnitType spawnType1;
    [SerializeField] UnitType spawnType2;
    [SerializeField] UnitType spawnType3;
    [SerializeField] Define.EnemyType enemyType;

    public int Dev_ID => dev_ID; // lv
    public int SpawnCount => spawnCount;
    public float SpawnDelayTime => spawnDelayTime;
    public float CountDownTime => countDownTime;
    public Define.EnemyType EnemyType => enemyType;

    private List<UnitType> spawnTypes;

    public List<UnitType> SpawnTypes
    {
        get
        {
            if(spawnTypes == null)
            {
                spawnTypes = new List<UnitType>();

                CheckSpawnType(spawnType1);
                CheckSpawnType(spawnType2);
                CheckSpawnType(spawnType3);
            }

            return spawnTypes;
        }
    }

    private void CheckSpawnType(UnitType type)
    {
        if (type != UnitType.None)
            SpawnTypes.Add(type);
    }
}
