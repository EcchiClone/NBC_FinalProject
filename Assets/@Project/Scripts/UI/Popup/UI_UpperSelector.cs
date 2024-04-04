using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperSelector : UI_Popup
{
    [SerializeField] GameObject _nextGroup;
    [SerializeField] TextMeshProUGUI[] _specTexts;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts;
    [SerializeField] private Transform _contents;

    enum Buttons
    {
        BackToSelector,
    }

    enum SpecType
    {
        AP,
        Weight,
        AttackMain,
        AttackSub,
        ReloadSub,
        RotateSpeed,
    }    

    protected override void Init()
    {
        base.Init();

        int createUI = Managers.Module.UpperPartsCount;        

        for (int i = 0; i < createUI; i++)
            Managers.UI.ShowItemUI<UI_UpperChangeBtn>(_contents).SetParentUI(this);

        Managers.Module.OnUpperChange += UpdateSelectedPartSpecText;
        ResetText();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
    }

    private void BackToSelector()
    {
        Managers.ActionManager.CallUndoMenuCam(Define.CamType.Upper);

        ResetText();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void ResetText()
    {
        int partID = Managers.Module.GetPartOfIndex<UpperPart>(0).ID;
        PartData currentPartData = Managers.Data.GetPartData(partID);

        UpdateSelectedPartSpecText(currentPartData);
    }

    private void UpdateSelectedPartSpecText(PartData upperData)
    {
        _nextGroup.SetActive(false);
        _specTexts[(int)SpecType.AP].text = $"{upperData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{upperData.Weight}";

        //_specTexts[(int)SpecType.AttackMain].text = $"{Managers.Module.CurrentUpperPart.Primary.WeaponSO.atk}";
        //_specTexts[(int)SpecType.AttackSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.atk}";
        //_specTexts[(int)SpecType.ReloadSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.coolDownTime}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{upperData.SmoothRotation}";
    }

    public void DisPlayNextPartSpecText(PartData nextUpperData)
    {
        _nextGroup.SetActive(true);
        _nextSpecTexts[(int)SpecType.AP].text = $"{nextUpperData.Armor}";
        _nextSpecTexts[(int)SpecType.Weight].text = $"{nextUpperData.Weight}";

        //_nextSpecTexts[(int)SpecType.AttackMain].text = $"{Managers.Module.CurrentUpperPart.Primary.WeaponSO.atk}";
        //_nextSpecTexts[(int)SpecType.AttackSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.atk}";
        //_nextSpecTexts[(int)SpecType.ReloadSub].text = $"{Managers.Module.CurrentUpperPart.Secondary.WeaponSO.coolDownTime}";
        _nextSpecTexts[(int)SpecType.RotateSpeed].text = $"{nextUpperData.SmoothRotation}";
    }
}
