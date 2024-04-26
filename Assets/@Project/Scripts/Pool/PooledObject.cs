using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    void OnDisable() // ObjectPooler 사용 시 필수 요소
    {
        if (!Managers.Pool.IsCleared)
            Managers.Pool.GetPooler(PoolingType.Enemy).ReturnToPool(gameObject);    // 한 객체에 한번만         
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }
}
