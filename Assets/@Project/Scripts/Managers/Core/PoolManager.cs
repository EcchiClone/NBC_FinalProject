using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Dictionary<PoolingType, ObjectPooler> PoolDict = new Dictionary<PoolingType, ObjectPooler>();

    public void SetPooler(ObjectPooler pooler) => PoolDict.Add(pooler.PoolingType, pooler);
    public ObjectPooler GetPooler(PoolingType type) => PoolDict[type];
    public void Clear()
    {
        IsCleared = true;
        PoolDict.Clear();
    }
    public bool IsCleared { get; set; }
}
