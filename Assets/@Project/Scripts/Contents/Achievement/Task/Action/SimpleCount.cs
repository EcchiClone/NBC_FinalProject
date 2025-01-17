using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Action/SimpleCount", fileName = "Simple Count")]
public class SimpleCount : TaskAction
{
    public override int Run(AchievementTask task, int currentSuccess, int successCount)
    {
        return currentSuccess + successCount;
    }
}
