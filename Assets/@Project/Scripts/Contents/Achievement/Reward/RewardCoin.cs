using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/Coin", fileName = "Reward_Coin")]
public class RewardCoin : AchievementReward
{
    public override void Give(Achievement achievement)
    {
        Debug.Log($"업적코인을 {Quantity} 만큼 획득했다!");
    }

}
