using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Action/SimpleMax", fileName = "Simple Max")]
public class SimpleMax : TaskAction
{
    public override int Run(AchievementTask task, int currentSuccess, int successCount)
    {
        return Mathf.Max(currentSuccess, successCount);
    }
}
