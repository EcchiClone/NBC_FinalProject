using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ModuleStatus
{
    #region Common
    public float Armor { get; private set; } // HP
    public float Weight { get; private set; }    
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

    public float CurrentArmor { get; private set; }
    public float CurrentBooster { get; private set; }
    public float ScanRangeAdjust { get; private set; }
    public float Defence { get; private set; }
    public float Stealth { get; private set; }

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

        PerkData perkData = Managers.GameManager.PerkData;

        Armor = lowerData.Armor + upperData.Armor + leftArmData.Armor + rightArmData.Armor + leftShoulderData.Armor + rightShoulderData.Armor;
        Armor += perkData.GetAbilityValue(PerkType.SuperAlloy);
        Weight = lowerData.Weight + upperData.Weight + leftArmData.Weight + rightArmData.Weight + leftShoulderData.Weight + rightShoulderData.Weight;
        
        MovementSpeed = lowerData.Speed * Util.GetIncreasePercentagePerkValue(perkData, PerkType.SpeedModifier);
        JumpPower = lowerData.JumpPower * Util.GetIncreasePercentagePerkValue(perkData, PerkType.Spring);
        CanJump = lowerData.CanJump;
        BoostPower = lowerData.BoosterPower * Util.GetIncreasePercentagePerkValue(perkData, PerkType.AfterBurner);

        SmoothRotateValue = upperData.SmoothRotation * Util.GetIncreasePercentagePerkValue(perkData, PerkType.Lubrication);
        BoosterGauge = upperData.BoosterGauge * Util.GetIncreasePercentagePerkValue(perkData, PerkType.BoosterOverload); 
        VTOL = upperData.Hovering * Util.GetIncreasePercentagePerkValue(perkData, PerkType.Jetpack);

        ScanRangeAdjust = Util.GetIncreasePercentagePerkValue(perkData, PerkType.Rador);
        Defence = Util.GetReducePercentagePerkValue(perkData, PerkType.ImprovedArmor);
        Stealth = perkData.GetAbilityValue(PerkType.Stealth);

        CurrentArmor = Armor;
        CurrentBooster = BoosterGauge;        
    }    

    public void GetDamage(float damage)
    {
        float random = Random.Range(0f, 100f);
        if (random <= Stealth)
            return;

        CurrentArmor -= damage * Defence;
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
