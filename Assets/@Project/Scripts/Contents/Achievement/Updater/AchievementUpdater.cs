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
        Managers.AchievementSystem.ReceiveReport(taskCategory, taskTarget, 1);
    }
    protected void Report(int value)
    {
        Managers.AchievementSystem.ReceiveReport(taskCategory, taskTarget, value);
    }
    protected void Report(string taskTarget, int value)
    {
        Managers.AchievementSystem.ReceiveReport(taskCategory, taskTarget, value);
    }
    protected void Report(string taskCategory, string taskTarget, int value)
    {
        Managers.AchievementSystem.ReceiveReport(taskCategory, taskTarget, value);
    }
    private void OnApplicationQuit()
    {
        Managers.AchievementSystem.SaveOnQuit();
    }

}
