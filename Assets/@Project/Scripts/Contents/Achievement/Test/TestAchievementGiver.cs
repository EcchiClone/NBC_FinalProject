using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAchievementGiver : MonoBehaviour
{
    [SerializeField]
    private Achievement[] achievements;

    private void Start()
    {
        GiveAchievements();
    }
    public void GiveAchievements() // DB SO 파일로부터 초기화
    {

        foreach (var achievement in achievements)
        {
            Managers.AchievementSystem.Register(achievement);
        }

    }
}
