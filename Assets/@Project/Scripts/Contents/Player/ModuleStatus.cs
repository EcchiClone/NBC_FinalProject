using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleStatus
{
    // To Do - SO 받아서 기본 능력치 Setup 하기

    // # Common Stats
    public float Armor { get; private set; } // HP
    public float Weight { get; private set; }

    // # Lower Stats
    public float MovementSpeed { get; private set; }
    public float JumpPower { get; private set; }
    public float BoostPower { get; private set; }
    public bool CanJump { get; private set; }

    // # Upper Stats
    public float AttackPrimary { get; private set; }
    public float AttackSecondary { get; private set; }
    public float ReloadSecondary { get; private set; }
    public float SmoothRotateValue { get; private set; }

    public static event Action<float, float> OnChangeArmorPoint;

    private float _currentArmor;

    public bool IsDead { get; private set; } = false;

    public ModuleStatus(LowerPart lower, UpperPart upper, WeaponPart leftArm, WeaponPart rightArm, WeaponPart leftShoulder, WeaponPart rightShoulder)
    {
        PartData lowerData = Managers.Data.GetPartData(lower.ID);
        PartData upperData = Managers.Data.GetPartData(upper.ID);
        PartData leftArmData = Managers.Data.GetPartData(leftArm.ID);
        PartData rightArmData = Managers.Data.GetPartData(rightArm.ID);
        PartData leftShoulderData = Managers.Data.GetPartData(leftShoulder.ID);
        PartData rightShoulderData = Managers.Data.GetPartData(rightShoulder.ID);

        Armor = lowerData.Armor + upperData.Armor;
        Weight = lowerData.Weight + upperData.Weight + leftArmData.Weight + rightArmData.Weight + leftShoulderData.Weight + rightShoulderData.Weight;
        _currentArmor = Armor;

        MovementSpeed = lowerData.Speed;
        JumpPower = lowerData.JumpPower;
        CanJump = lowerData.CanJump;
        BoostPower = lowerData.BoosterPower;

        SmoothRotateValue = upperData.SmoothRotation;

        OnChangeArmorPoint?.Invoke(Armor, _currentArmor);
    }

    public void GetDamage(float damage)
    {
        _currentArmor -= damage;
        if (_currentArmor <= 0)
            Dead();
        OnChangeArmorPoint?.Invoke(Armor, _currentArmor);
    }

    public void Repair()
    {
        _currentArmor = Mathf.Min(_currentArmor + 250, Armor);
        OnChangeArmorPoint?.Invoke(Armor, _currentArmor);
    }

    private void Dead()
    {
        if (IsDead)
            return;

        Managers.ActionManager.CallPlayerDead();
    }
}
