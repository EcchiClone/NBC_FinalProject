using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementGiver : MonoBehaviour
{
    [SerializeField]
    private Achievement[] achievements;

    private void Start()
    {
        GiveAchievements();
    }
    public void GiveAchievements()
    {
        foreach (var achievement in achievements)
        {
            if (!AchievementSystem.Instance.ContainsInCompletedAchievements(achievement) && !AchievementSystem.Instance.ContainsInActiveAchievements(achievement))
                AchievementSystem.Instance.Register(achievement);
        }
    }
}
