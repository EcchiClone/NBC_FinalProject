using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UI_SelectorMenu : UI_Popup
{
    private UI_Popup[] _partsMenus = new UI_Popup[2];
    private UnityAction _camAction;

    [SerializeField] TextMeshProUGUI[] _specTexts;

    enum Buttons
    {
        UpperParts_Btn,
        LowerParts_Btn,
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

    private float lowerAP;
    private float upperAP;
    private float lowerWeight;
    private float upperWeight;

    protected override void Init()
    {
        base.Init();

        Managers.Module.OnLowerSpecsChange += ApplyLowerModuleSpec;
        Managers.Module.OnUpperSpecsChange += ApplyUpperModuleSpec;        
        InitSpecTexts();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackToMain).onClick.AddListener(BackToMain);
        GetButton((int)Buttons.UpperParts_Btn).onClick.AddListener(() => OpenUpperParts<UI_UpperSelector>((int)Buttons.UpperParts_Btn));
        GetButton((int)Buttons.LowerParts_Btn).onClick.AddListener(() => OpenUpperParts<UI_LowerSelector>((int)Buttons.LowerParts_Btn));
    }

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
    }

    private void OpenUpperParts<T>(int index) where T : UI_Popup
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
        lowerAP = Managers.Module.CurrentLowerPart.lowerSO.armor;
        lowerWeight = Managers.Module.CurrentLowerPart.lowerSO.weight;        

        upperAP = Managers.Module.CurrentUpperPart.upperSO.armor;
        upperWeight = Managers.Module.CurrentUpperPart.upperSO.weight;

        float attakMain = Managers.Module.CurrentUpperPart.Primary.WeaponSO.atk;
        float attakSub = Managers.Module.CurrentUpperPart.Secondary.WeaponSO.atk;
        float reloadSub = Managers.Module.CurrentUpperPart.Secondary.WeaponSO.coolDownTime;
        float rotSpeed = Managers.Module.CurrentUpperPart.upperSO.smoothRotation;

        float moveSpeed = Managers.Module.CurrentLowerPart.lowerSO.speed;        
        float jumpPower = Managers.Module.CurrentLowerPart.lowerSO.jumpPower;
        float boostPower = Managers.Module.CurrentLowerPart.lowerSO.boosterPower;        

        _specTexts[(int)SpecType.AP].text = $"{lowerAP + upperAP}";
        _specTexts[(int)SpecType.Weight].text = $"{lowerWeight + upperWeight}";
        _specTexts[(int)SpecType.AttackMain].text = $"{attakMain}";
        _specTexts[(int)SpecType.AttackSub].text = $"{attakSub}";
        _specTexts[(int)SpecType.ReloadSub].text = $"{reloadSub}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{moveSpeed}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{rotSpeed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{jumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{boostPower}";
    }

    private void ApplyLowerModuleSpec(float ap, float weight, float moveSpeed, float jumpPower, float boostPower)
    {
        lowerAP = ap;
        lowerWeight = weight;

        _specTexts[(int)SpecType.AP].text = $"{upperAP + ap}";
        _specTexts[(int)SpecType.Weight].text = $"{upperWeight + weight}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{moveSpeed}";        
        _specTexts[(int)SpecType.JumpPower].text = $"{jumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{boostPower}";
    }

    private void ApplyUpperModuleSpec(float ap, float weight, float attackP, float attacS, float coolDownS, float rotSpeed)
    {
        upperAP = ap;
        upperWeight = weight;

        _specTexts[(int)SpecType.AP].text = $"{lowerAP + ap}";
        _specTexts[(int)SpecType.Weight].text = $"{lowerWeight + weight}";

        _specTexts[(int)SpecType.AttackMain].text = $"{attackP}";
        _specTexts[(int)SpecType.AttackSub].text = $"{attacS}";
        _specTexts[(int)SpecType.ReloadSub].text = $"{coolDownS}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{rotSpeed}";
    }
}
