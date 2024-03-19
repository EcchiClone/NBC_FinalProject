using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    // To Do - SO 받아서 기본 능력치 Setup 하기

    // # Common Stats
    public float Armor { get; private set; }
    public float Weight { get; private set; }

    // # Lower Stats
    public float MovementSpeed { get; private set; }
    public float JumpPower { get; private set; } // 점프 높이    
    public bool CanJump { get; private set; }

    // # Upper Stats
    public float SmoothRotateValue { get; private set; }
    public float BoostPower { get; private set; } // 부스터 출력  

    public static event Action<float, float> OnChangeArmorPoint;

    private float _currentArmor;

    public PlayerStatus(LowerPartsSO lowerSO, UpperPartsSO upperSO)
    {
        Armor = lowerSO.armor + upperSO.armor;        
        Weight = lowerSO.weight + upperSO.weight;
        _currentArmor = Armor;

        MovementSpeed = lowerSO.speed;
        JumpPower = lowerSO.jumpPower;
        CanJump = lowerSO.canJump;

        SmoothRotateValue = upperSO.smoothRotation;
        BoostPower = upperSO.boosterPower;        
    }

    public void GetDamage(float damage)
    {
        _currentArmor -= damage;        
        OnChangeArmorPoint?.Invoke(Armor, _currentArmor);
    }
}
