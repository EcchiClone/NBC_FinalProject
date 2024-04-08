using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementCondition : ScriptableObject
{
    [SerializeField]
    private string description;

    public abstract bool IsPass(Achievement achievement);
}
