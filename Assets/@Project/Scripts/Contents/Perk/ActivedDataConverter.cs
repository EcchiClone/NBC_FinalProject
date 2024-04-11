using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivedDataConverter : MonoBehaviour
{
    public List<ActivedData> activeData;
}

[System.Serializable]
public class ActivedData
{
    // 모듈러 양식
    public float SpeedModular;
    public float TestVar1;
    public float TestVar2;
    // ... 이하 지윤님 스크립트에 맞춰 계속 추가할 것

    public ActivedData
        // 디폴트 값
        (float speedModular = 1f, float testVar1 = 0, float testVar2 = 0)
    {
        SpeedModular = speedModular;
        TestVar1 = testVar1;
        TestVar2 = testVar2;
    }
}
