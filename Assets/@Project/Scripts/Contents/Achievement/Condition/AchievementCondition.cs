using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementCondition : ScriptableObject
{
    // 업적 받는 조건 쓰는 목적이었지만 미사용
    [SerializeField]
    private string description;

    public abstract bool IsPass(Achievement achievement);
}
