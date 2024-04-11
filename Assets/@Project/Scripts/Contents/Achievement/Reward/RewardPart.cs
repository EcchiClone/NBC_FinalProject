using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/Part", fileName = "Reward_Part")]
public class RewardPart : AchievementReward
{
    public override void Give(Achievement achievement)
    {
        Debug.Log($"파츠 '{QuantityOrValue}'를 획득했다!");
        Managers.GameManager.ReceivePartID(quantityOrValue);
    }

}
