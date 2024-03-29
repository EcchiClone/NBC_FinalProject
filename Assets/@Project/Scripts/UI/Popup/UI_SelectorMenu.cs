using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class UI_SelectorMenu : UI_Popup
{
    private UI_Popup[] _partsMenus = new UI_Popup[4];
    private UnityAction _camAction;

    [SerializeField] TextMeshProUGUI[] _specTexts;

    enum Buttons
    {
        UpperParts_Btn,
        LowerParts_Btn,
        ArmWeaponParts_Btn,
        ShoulderWeaponParts_Btn,
        BackToMain,
    }

    enum SpecType
    {
        AP,
        Weight,
        AttackMain,
        AttackSub,
        ReloadSub,
        MoveSpeed,
        RotateSpeed,
        JumpPower,
        BoostPower,
    }

    private PartData _lowerData;
    private PartData _upperData;

    private float lowerAP;
    private float upperAP;
    private float lowerWeight;
    private float upperWeight;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnLowerChange += ApplyLowerModuleSpec;
        Managers.Module.OnUpperChange += ApplyUpperModuleSpec;
        InitSpecTexts();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackToMain).onClick.AddListener(BackToMain);
        GetButton((int)Buttons.UpperParts_Btn).onClick.AddListener(() => OpenParts<UI_UpperSelector>((int)Buttons.UpperParts_Btn));
        GetButton((int)Buttons.LowerParts_Btn).onClick.AddListener(() => OpenParts<UI_LowerSelector>((int)Buttons.LowerParts_Btn));
        GetButton((int)Buttons.ArmWeaponParts_Btn).onClick.AddListener(() => OpenParts<UI_ArmSelector>((int)Buttons.ArmWeaponParts_Btn));
        GetButton((int)Buttons.ShoulderWeaponParts_Btn).onClick.AddListener(() => OpenParts<UI_ShoulderSelector>((int)Buttons.ShoulderWeaponParts_Btn));
    }

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
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

        gameObject.SetActive(false);
    }

    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        _camAction.Invoke();
        gameObject.SetActive(false);
    }

    private void InitSpecTexts()
    {
        InitData initData = new InitData();

        _lowerData = Managers.Data.GetPartData(initData.LowerPartId[0]);
        _upperData = Managers.Data.GetPartData(initData.UpperPartId[0]);

        //float attakMain = Managers.Module.CurrentUpperPart.Primary.WeaponSO.atk;
        //float attakSub = Managers.Module.CurrentUpperPart.Secondary.WeaponSO.atk;
        //float reloadSub = Managers.Module.CurrentUpperPart.Secondary.WeaponSO.coolDownTime;
        float rotSpeed = _upperData.SmoothRotation;

        float moveSpeed = _lowerData.Speed;
        float jumpPower = _lowerData.JumpPower;
        float boostPower = _lowerData.BoosterPower;

        _specTexts[(int)SpecType.AP].text = $"{_lowerData.Armor + _upperData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{_lowerData.Weight + _upperData.Weight}";
        //_specTexts[(int)SpecType.AttackMain].text = $"{attakMain}";
        //_specTexts[(int)SpecType.AttackSub].text = $"{attakSub}";
        //_specTexts[(int)SpecType.ReloadSub].text = $"{reloadSub}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{moveSpeed}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{rotSpeed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{jumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{boostPower}";
    }

    private void ApplyLowerModuleSpec(PartData lowerData)
    {
        _lowerData = lowerData;

        _specTexts[(int)SpecType.AP].text = $"{upperAP + lowerData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{upperWeight + lowerData.Weight}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{lowerData.Speed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{lowerData.JumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{lowerData.BoosterPower}";
    }

    private void ApplyUpperModuleSpec(PartData upperData)
    {
        _upperData = upperData;

        _specTexts[(int)SpecType.AP].text = $"{lowerAP + upperData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{lowerWeight + upperData.Weight}";

        //_specTexts[(int)SpecType.AttackMain].text = $"{Managers.Module.CurrentUpperPart.Primary.WeaponSO.atk}";
        //_specTexts[(int)SpecType.AttackSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.atk}";
        //_specTexts[(int)SpecType.ReloadSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.coolDownTime}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{upperData.SmoothRotation}";
    }
}
