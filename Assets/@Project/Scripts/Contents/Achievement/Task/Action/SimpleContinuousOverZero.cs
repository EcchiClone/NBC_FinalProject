using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Action/SimpleContinuousOverZero", fileName = "Action_SimpleContinuousOverZero")]
public class SimpleContinuousOverZero: TaskAction
{
    public override int Run(AchievementTask task, int currentSuccess, int successCount)
    {
        if (successCount > 0)
            return currentSuccess + successCount;
        else
            return 0;
    }
}
