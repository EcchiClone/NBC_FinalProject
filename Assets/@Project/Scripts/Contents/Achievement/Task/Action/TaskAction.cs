using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskAction : ScriptableObject
{
    public abstract int Run(AchievementTask task, int currentSuccess, int successCount);
}
