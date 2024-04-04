using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementReward : ScriptableObject
{
    [SerializeField]
    private Sprite icon; // 코인 아이콘, 파츠 아이콘
    [SerializeField]
    private string description;
    [SerializeField]
    private int quantity; 

    public Sprite Icon => icon;
    public string Description => description;
    public int Quantity => quantity;

    public abstract void Give(Achievement achievement);
    // Todo : 실제로 보상을 주는 CoinReward, PartReward 스크립트 작성
}
