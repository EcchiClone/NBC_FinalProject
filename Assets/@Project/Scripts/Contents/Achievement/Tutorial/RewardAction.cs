using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Reward/Action", fileName = "Reward_Action")]
public class RewardAction : AchievementReward
{
    public override void Give(Achievement achievement)
    {
        Managers.Tutorial.NextPhase();
        Debug.Log("보상 줬음");
    }
}
