using System;
using UnityEngine;
using UnityEngine.Events;

public class ModuleStatus
{
    #region Common
    public float Armor { get; private set; } // HP
    public float Weight { get; private set; }

    public float DEF { get; private set; } // PerkOnly
    #endregion

    #region Lower
    public float MovementSpeed { get; private set; }
    public float JumpPower { get; private set; }
    public float BoostPower { get; private set; }
    public bool CanJump { get; private set; }
    #endregion

    #region Upper
    public float SmoothRotateValue { get; private set; }
    public float BoosterGauge { get; private set; }
    public float VTOL { get; private set; }
    #endregion

    #region Weapon
    
    #endregion

    

    public float CurrentArmor { get; private set; }
    public float CurrentBooster { get; private set; }

    public bool IsDead { get; private set; } = false;

    private readonly float DASH_BOOSTER_CONSUME = 20f;
    private readonly float HOVER_BOOSTER_CONSUME = 100f;

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

        MovementSpeed = lowerData.Speed;
        JumpPower = lowerData.JumpPower;
        CanJump = lowerData.CanJump;
        BoostPower = lowerData.BoosterPower;

        SmoothRotateValue = upperData.SmoothRotation;
        BoosterGauge = upperData.BoosterGauge;
        VTOL = upperData.Hovering;

        CurrentArmor = Armor;
        CurrentBooster = BoosterGauge;

        Managers.ModuleActionManager.CallChangeArmorPoint(Armor, CurrentArmor);
        Managers.ModuleActionManager.CallChangeBoosterGauge(BoosterGauge, CurrentBooster);
    }

    public void GetDamage(float damage)
    {
        CurrentArmor -= damage - DEF;
        if (CurrentArmor <= 0)
            Dead();
        Managers.ModuleActionManager.CallChangeArmorPoint(Armor, CurrentArmor);
    }

    public void Repair()
    {
        CurrentArmor = Mathf.Min(CurrentArmor + 250, Armor);
        Managers.ModuleActionManager.CallChangeArmorPoint(Armor, CurrentArmor);
    }

    public bool Boost()
    {
        if (CurrentBooster < DASH_BOOSTER_CONSUME)
            return false;

        CurrentBooster = Mathf.Max(0, CurrentBooster - DASH_BOOSTER_CONSUME);
        Managers.ModuleActionManager.CallChangeBoosterGauge(BoosterGauge, CurrentBooster);
        return true;
    }

    public void Hovering(UnityAction action)
    {
        if (CurrentBooster <= 0)
            return;

        CurrentBooster = Mathf.Max(0, CurrentBooster - HOVER_BOOSTER_CONSUME * Time.deltaTime);
        action.Invoke();
        Managers.ModuleActionManager.CallChangeBoosterGauge(BoosterGauge, CurrentBooster);
    }

    public void BoosterRecharge()
    {
        CurrentBooster = Mathf.Min(BoosterGauge, CurrentBooster + 0.5f);
        Managers.ModuleActionManager.CallChangeBoosterGauge(BoosterGauge, CurrentBooster);
    }

    private void Dead()
    {
        if (IsDead)
            return;

        Managers.ActionManager.CallPlayerDead();
    }
}
