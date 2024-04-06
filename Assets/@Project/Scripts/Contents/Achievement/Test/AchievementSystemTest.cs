using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystemTest : MonoBehaviour
{
    [SerializeField]
    private Achievement[] achievements;
    [SerializeField]
    private TaskCategory category;
    [SerializeField]
    private TaskTarget target;

    void Start()
    {
        var achievementSystem = AchievementSystem.Instance;

        // 등록 알림 구독
        achievementSystem.onAchievementRegistered += (achievement) =>
        {
            print($"새 업적 [{achievement.CodeName}]을 등록했습니다");
        };

        // 완료 알림 구독
        achievementSystem.onAchievementCompleted += (achievement) =>
        {
            print($"업적 [{achievement.CodeName}]를 완료");
        };

        // 구독
        foreach (var achievement in achievements)
        {
            var newAchievement = achievementSystem.Register(achievement);
            newAchievement.onTaskSuccessChanged += (achievement, task, currentSuccess, prevSuccess) =>
            {
                print($"[{achievement.CodeName}/{task.CodeName}]의 현재 성공 수: {currentSuccess}");
            };
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            // 몬스터 잡는 등 카운트 갱신할 곳에 추가하면 될 듯.
            AchievementSystem.Instance.ReceiveReport(category, target, 1);
    }
}
