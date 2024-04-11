using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/AchievementPoint", fileName = "Reward_AchievementPoint")]
public class RewardCoin : AchievementReward
{
    public override void Give(Achievement achievement)
    {
        Debug.Log($"업적 포인트를 {QuantityOrValue} 만큼 획득했다!");
        Managers.GameManager.AchievementCoin += quantityOrValue;
        // To Do - [Call] 보상을 줬습니다. QuantityOrValue 만큼... 을
    }

}
