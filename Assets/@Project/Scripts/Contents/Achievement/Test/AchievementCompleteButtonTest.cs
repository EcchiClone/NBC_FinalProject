using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCompleteButtonTest : MonoBehaviour
{
    public Button receiveButton; // 수령하기 버튼
    public string achievementCodeName; // 수령하려는 업적의 코드 이름

    void Start()
    {
        receiveButton.onClick.AddListener(() =>
        {
            Debug.Log("버튼 클릭 이벤트 실행됨: " + achievementCodeName);
            AchievementSystem.instance.ReceiveRewardsAndCompleteAchievement(achievementCodeName);
        });
    }
}
