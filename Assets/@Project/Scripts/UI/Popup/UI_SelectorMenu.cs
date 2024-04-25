using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static Define;

public class UI_SelectorMenu : UI_Popup
{
    private UI_Popup[] _partsMenus = new UI_Popup[4];    

    [SerializeField] TextMeshProUGUI[] _specTexts;

    enum Buttons
    {
        LowerParts_Btn,
        UpperParts_Btn,        
        ArmWeaponParts_Btn,
        ShoulderWeaponParts_Btn,
        BackToMain,
    }

    enum SpecType
    {
        AP,
        Weight,
        MoveSpeed,
        JumpPower,
        HoverPower,
        Rotation,
        BoosterGauge,        
        BoostPower,
    }

    private PartData _lowerData;
    private PartData _upperData;
    private PartData _leftArmData;
    private PartData _rightArmData;
    private PartData _leftShoulderData;
    private PartData _rightShoulderData;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnLowerChange += ApplyChangedModuleSpecs;
        Managers.Module.OnUpperChange += ApplyChangedModuleSpecs;
        Managers.Module.OnLeftArmChange += ApplyChangedModuleSpecs;
        Managers.Module.OnRightArmChange += ApplyChangedModuleSpecs;
        Managers.Module.OnLeftShoulderChange += ApplyChangedModuleSpecs;
        Managers.Module.OnRightSoulderChange += ApplyChangedModuleSpecs;

        ApplyChangedModuleSpecs();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackToMain).onClick.AddListener(BackToMain);
        GetButton((int)Buttons.UpperParts_Btn).onClick.AddListener(() => OpenParts<UI_UpperSelector>((int)Buttons.UpperParts_Btn));
        GetButton((int)Buttons.LowerParts_Btn).onClick.AddListener(() => OpenParts<UI_LowerSelector>((int)Buttons.LowerParts_Btn));
        GetButton((int)Buttons.ArmWeaponParts_Btn).onClick.AddListener(() => OpenParts<UI_ArmSelector>((int)Buttons.ArmWeaponParts_Btn));
        GetButton((int)Buttons.ShoulderWeaponParts_Btn).onClick.AddListener(() => OpenParts<UI_ShoulderSelector>((int)Buttons.ShoulderWeaponParts_Btn));
    }

    private void OpenParts<T>(int index) where T : UI_Popup
    {
        if (_partsMenus[index] == null)
        {
            _partsMenus[index] = Managers.UI.ShowPopupUI<T>();
            _partsMenus[index].SetPreviousPopup(this);
        }
        else
            _partsMenus[index].gameObject.SetActive(true);

        Managers.ActionManager.CallSelectorCam((CamType)(index + 2));
        gameObject.SetActive(false);
    }

    private void BackToMain()
    {
        Managers.ActionManager.CallUndoMenuCam(CamType.Module);

        _previousPopup.gameObject.SetActive(true);           
        gameObject.SetActive(false);
    }

    private void ApplyChangedModuleSpecs(PartData data = null)
    {
        _lowerData = Managers.Data.GetPartData(Managers.Module.CurrentLowerPart.ID);
        _upperData = Managers.Data.GetPartData(Managers.Module.CurrentUpperPart.ID);
        _leftArmData = Managers.Data.GetPartData(Managers.Module.CurrentLeftArmPart.ID);
        _rightArmData = Managers.Data.GetPartData(Managers.Module.CurrentRightArmPart.ID);
        _leftShoulderData = Managers.Data.GetPartData(Managers.Module.CurrentLeftShoulderPart.ID);
        _rightShoulderData = Managers.Data.GetPartData(Managers.Module.CurrentRightShoulderPart.ID);

        _specTexts[(int)SpecType.AP].text = $"{_lowerData.Armor + _upperData.Armor + _leftArmData.Armor + _rightArmData.Armor + _leftShoulderData.Armor + _rightShoulderData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{_lowerData.Weight + _upperData.Weight + _leftArmData.Weight + _rightArmData.Weight + _leftShoulderData.Weight + _rightShoulderData.Weight}";
        _specTexts[(int)SpecType.MoveSpeed].text = $"{_lowerData.Speed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{_lowerData.JumpPower}";
        _specTexts[(int)SpecType.HoverPower].text = $"{_upperData.Hovering}";
        _specTexts[(int)SpecType.Rotation].text = $"{_upperData.SmoothRotation}";
        _specTexts[(int)SpecType.BoosterGauge].text = $"{_upperData.BoosterGauge}";
        _specTexts[(int)SpecType.BoostPower].text = $"{_lowerData.BoosterPower}";
    }
}
