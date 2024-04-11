using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Action/SimpleSet", fileName = "SimpleSet")]
public class SimpleSet : TaskAction
{
    public override int Run(AchievementTask task, int currentSuccess, int successCount)
    {
        return successCount;
    }
}
