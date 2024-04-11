using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AchievementUpdater : MonoBehaviour
{
    [SerializeField] protected TaskCategory taskCategory;
    [SerializeField] protected TaskTarget taskTarget;
    protected void Report()
    {
        AchievementSystem.Instance.ReceiveReport(taskCategory, taskTarget, 1);
    }
    protected void Report(int value)
    {
        AchievementSystem.Instance.ReceiveReport(taskCategory, taskTarget, value);
    }
    protected void Report(string taskTarget, int value)
    {
        AchievementSystem.Instance.ReceiveReport(taskCategory, taskTarget, value);
    }
    protected void Report(string taskCategory, string taskTarget, int value)
    {
        AchievementSystem.Instance.ReceiveReport(taskCategory, taskTarget, value);
    }
}
