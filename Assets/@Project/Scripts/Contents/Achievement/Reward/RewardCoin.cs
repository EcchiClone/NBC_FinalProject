using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/AchievementPoint", fileName = "Reward_AchievementPoint")]
public class RewardCoin : AchievementReward
{
    public override void Give(Achievement achievement)
    {
        Debug.Log($"업적 포인트를 {QuantityOrValue} 만큼 획득했다!");

    }

}
