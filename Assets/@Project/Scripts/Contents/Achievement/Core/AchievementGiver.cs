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
    public void GiveAchievements() // DB SO 파일로부터 초기화
    {
        //string DBPath = "Data/AchievementDatabase";
        //AchievementDatabase achievementDB = Resources.Load<AchievementDatabase>(DBPath);
        //if (achievementDB == null)
        //{
        //    Debug.LogError($"DB파일(SO) 경로가 잘못된 것 같습니다. 'Resources/{DBPath}'");
        //    return;
        //}

        //foreach (var achievement in achievementDB.achievements)
        //{
        //    // 각 업적을 AchievementSystem에 등록합니다.
        //    if (!Managers.AchievementSystem.ContainsInCompletedAchievements(achievement) &&
        //        !Managers.AchievementSystem.ContainsInActiveAchievements(achievement))
        //    {
        //        Managers.AchievementSystem.Register(achievement);
        //    }
        //}

        foreach (var achievement in achievements)
        {
            if (!Managers.AchievementSystem.ContainsInCompletedAchievements(achievement) && !Managers.AchievementSystem.ContainsInActiveAchievements(achievement))
                Managers.AchievementSystem.Register(achievement);
        }
    }
}
