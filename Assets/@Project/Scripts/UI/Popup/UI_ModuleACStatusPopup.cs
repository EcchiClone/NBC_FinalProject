using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ModuleACStatusPopup : UI_Popup
{
    enum statType
    {
        AP,
        DEF,
        MOVE_SPD,
        JUMP_POWER,
        BOOSTER_POWER,
        BOOSTER_GAUGE,
        HOVERING,
        SMOOTH_ROT,
        STEALTH,        
    }

    [SerializeField] TextMeshProUGUI[] _statusTexts;
    [SerializeField] Button _armPopup;
    [SerializeField] Button _shoulderPopup;

    protected override void Init()
    {
        base.Init();

        _armPopup.onClick.AddListener(() =>
        {
            Managers.UI.ShowPopupUI<UI_ModuleArmStatusPopup>(isStack: false);
            Close();
        });
        _shoulderPopup.onClick.AddListener(() =>
        {
            Managers.UI.ShowPopupUI<UI_ModuleShoulderStatusPopup>(isStack: false);
            Close();
        });

        PrintACStatus(); 
        UI_StageSelectPopup.OnPairPopup += Close;
    }

    private void Close()
    {
        Destroy(gameObject);
        UI_StageSelectPopup.OnPairPopup -= Close;
    }

    private void PrintACStatus()
    {
        ModuleStatus status = Managers.Module.CurrentModule.ModuleStatus;
        PerkData perkData = Managers.GameManager.PerkData;

        _statusTexts[(int)statType.AP].text = $"{status.Armor} [{status.Armor - AbilityValue(PerkType.SuperAlloy)}] [<color=green>+{AbilityValue(PerkType.SuperAlloy)}</color>]";
        _statusTexts[(int)statType.DEF].text = $"데미지 경감 <color=green>+{AbilityValue(PerkType.ImprovedArmor)}</color>%";
        _statusTexts[(int)statType.MOVE_SPD].text = $"{status.MovementSpeed} [{status.MovementSpeed / Util.GetIncreasePercentagePerkValue(perkData, PerkType.SpeedModifier)}] [<color=green>+{AbilityValue(PerkType.SpeedModifier)}%</color>]";
        _statusTexts[(int)statType.JUMP_POWER].text = $"{status.JumpPower} [{status.JumpPower / Util.GetIncreasePercentagePerkValue(perkData, PerkType.Spring)}] [<color=green>+{AbilityValue(PerkType.Spring)}%</color>]";
        _statusTexts[(int)statType.BOOSTER_POWER].text = $"{status.BoostPower} [{status.BoostPower / Util.GetIncreasePercentagePerkValue(perkData, PerkType.AfterBurner)}] [<color=green>+{AbilityValue(PerkType.AfterBurner)}%</color>]";
        _statusTexts[(int)statType.BOOSTER_GAUGE].text = $"{status.BoosterGauge} [{status.BoosterGauge / Util.GetIncreasePercentagePerkValue(perkData, PerkType.BoosterOverload)}] [<color=green>+{AbilityValue(PerkType.BoosterOverload)}%</color>]";
        _statusTexts[(int)statType.HOVERING].text = $"{status.VTOL} [{status.VTOL / Util.GetIncreasePercentagePerkValue(perkData, PerkType.Jetpack)}] [<color=green>+{AbilityValue(PerkType.Jetpack)}%</color>]";
        _statusTexts[(int)statType.SMOOTH_ROT].text = $"{status.SmoothRotateValue} [{status.SmoothRotateValue / Util.GetIncreasePercentagePerkValue(perkData, PerkType.Lubrication)}] [<color=green>+{AbilityValue(PerkType.Lubrication)}%</color>]";
        _statusTexts[(int)statType.STEALTH].text = $"{status.Stealth}%";        
    }

    private float AbilityValue(PerkType type)
    {
        PerkData perkData = Managers.GameManager.PerkData;

        return perkData.GetAbilityValue(type);
    }
}
