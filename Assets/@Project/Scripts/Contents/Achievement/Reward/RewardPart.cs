using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/Part", fileName = "Reward_Part")]
public class RewardPart : AchievementReward
{
    public int partNum;
    public override void Give(Achievement achievement)
    {
        Debug.Log($"파츠 '{partNum}'를 획득했다!");
    }

}
