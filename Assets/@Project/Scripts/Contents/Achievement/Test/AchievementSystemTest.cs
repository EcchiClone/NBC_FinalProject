using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystemTest : MonoBehaviour
{
    [SerializeField]
    private Achievement achievement;
    [SerializeField]
    private TaskCategory category;
    [SerializeField]
    private TaskTarget target;

    void Start()
    {
        var achievementSystem = AchievementSystem.Instance;

        achievementSystem.onAchievementRegistered += (achievement) =>
        {
            print($"New Achievement:{achievement.CodeName} Registered");
            print($"Active Achievements Count:{achievementSystem.ActiveAchievements.Count}");
        };

        achievementSystem.onAchievementCompleted += (achievement) =>
        {
            print($"Achievement:{achievement.CodeName} Completed");
            print($"Completed Achievements Count:{achievementSystem.CompletedAchievements.Count}");
        };

        var newAchievement = achievementSystem.Register(achievement);
        newAchievement.onTaskSuccessChanged += (achievement, task, currentSuccess, prevSuccess) =>
        {
            print($"Achievement:{achievement.CodeName}, Task:{task.CodeName}, CurrentSuccess:{currentSuccess}");
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            // 몬스터 잡는 등 카운트 갱신할 곳에 추가하면 될 듯.
            AchievementSystem.Instance.ReceiveReport(category, target, 1);
    }
}
