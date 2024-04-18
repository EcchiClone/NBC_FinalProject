using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ModuleShoulderStatusPopup : UI_Popup
{
    enum statType
    {
        L_DMG,
        L_FIRE_RATE,
        L_BULLET_SPD,
        L_AMMO,
        L_PERSHOT,
        L_RELOADABLE,        

        R_DMG,
        R_FIRE_RATE,
        R_BULLET_SPD,
        R_AMMO,
        R_PERSHOT,
        R_RELOADABLE,        
    }

    [SerializeField] TextMeshProUGUI[] _statusTexts;
    [SerializeField] Button _acPopup;
    [SerializeField] Button _armPopup;

    protected override void Init()
    {
        base.Init();

        _acPopup.onClick.AddListener(() =>
        {
            Managers.UI.ShowPopupUI<UI_ModuleACStatusPopup>(isStack: false);
            Close();
        });
        _armPopup.onClick.AddListener(() =>
        {
            Managers.UI.ShowPopupUI<UI_ModuleArmStatusPopup>(isStack: false);
            Close();
        });

        PrintArmStatus();
        UI_StageSelectPopup.OnPairPopup += Close;
    }    

    private void Close()
    {
        Destroy(gameObject);
        UI_StageSelectPopup.OnPairPopup -= Close;
    }

    private void PrintArmStatus()
    {
        WeaponBase arm_L = Managers.Module.CurrentModule.CurrentLeftShoulder.GetComponent<WeaponBase>();
        WeaponBase arm_R = Managers.Module.CurrentModule.CurrentRightShoulder.GetComponent<WeaponBase>();
        PerkData perkData = Managers.GameManager.PerkData;

        _statusTexts[(int)statType.L_DMG].text = $"{arm_L.Damage} [{arm_L.Damage / Util.GetIncreasePercentagePerkValue(perkData, PerkType.ImprovedBullet)}] [<color=green>+{AbilityValue(PerkType.ImprovedBullet)}%</color>]";
        _statusTexts[(int)statType.L_FIRE_RATE].text = $"{arm_L.FireRate} [{arm_L.FireRate / Util.GetIncreasePercentagePerkValue(perkData, PerkType.OverHeat)}] [<color=green>+{AbilityValue(PerkType.OverHeat)}%</color>]";
        _statusTexts[(int)statType.L_BULLET_SPD].text = $"{arm_L.BulletSpeed} [{arm_L.BulletSpeed / Util.GetIncreasePercentagePerkValue(perkData, PerkType.RapidFire)}] [<color=green>+{AbilityValue(PerkType.RapidFire)}%</color>]";
        _statusTexts[(int)statType.L_AMMO].text = $"{arm_L.Ammo} [{arm_L.Ammo / Util.GetIncreasePercentagePerkValue(perkData, PerkType.SpareAmmunition)}] [<color=green>+{AbilityValue(PerkType.SpareAmmunition)}%</color>]";
        _statusTexts[(int)statType.L_PERSHOT].text = $"{arm_L.PerShot}";
        _statusTexts[(int)statType.L_RELOADABLE].text = arm_L.CanReload ? "<color=green>ON</color>" : "<color=red>OFF</color>";        

        _statusTexts[(int)statType.R_DMG].text = $"{arm_R.Damage} [{arm_R.Damage / Util.GetIncreasePercentagePerkValue(perkData, PerkType.ImprovedBullet)}] [<color=green>+{AbilityValue(PerkType.ImprovedBullet)}%</color>]";
        _statusTexts[(int)statType.R_FIRE_RATE].text = $"{arm_R.FireRate} [{arm_R.FireRate / Util.GetIncreasePercentagePerkValue(perkData, PerkType.OverHeat)}] [<color=green>+{AbilityValue(PerkType.OverHeat)}%</color>]";
        _statusTexts[(int)statType.R_BULLET_SPD].text = $"{arm_R.BulletSpeed} [{arm_R.BulletSpeed / Util.GetIncreasePercentagePerkValue(perkData, PerkType.RapidFire)}] [<color=green>+{AbilityValue(PerkType.RapidFire)}%</color>]";
        _statusTexts[(int)statType.R_AMMO].text = $"{arm_R.Ammo} [{arm_R.Ammo / Util.GetIncreasePercentagePerkValue(perkData, PerkType.SpareAmmunition)}] [<color=green>+{AbilityValue(PerkType.SpareAmmunition)}%</color>]";
        _statusTexts[(int)statType.R_PERSHOT].text = $"{arm_R.PerShot}";
        _statusTexts[(int)statType.R_RELOADABLE].text = arm_R.CanReload ? "<color=green>ON</color>" : "<color=red>OFF</color>";
    }

    private float AbilityValue(PerkType type)
    {
        PerkData perkData = Managers.GameManager.PerkData;

        return perkData.GetAbilityValue(type);
    }
}
