using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperSelector : UI_Popup
{
    [SerializeField] TextMeshProUGUI[] _specTexts;

    enum SpecType
    {
        AP,
        Weight,
        AttackMain,
        AttackSub,
        ReloadSub,        
        RotateSpeed,        
    }

    [SerializeField] private Transform _contents;    

    enum Buttons
    {        
        BackToSelector,
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
        ResetText();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ResetText()
    {
        _specTexts[(int)SpecType.AP].text = "";
        _specTexts[(int)SpecType.Weight].text = "";

        _specTexts[(int)SpecType.AttackMain].text = "";
        _specTexts[(int)SpecType.AttackSub].text = "";
        _specTexts[(int)SpecType.ReloadSub].text = "";
        _specTexts[(int)SpecType.RotateSpeed].text = "";
    }

    private void UpdateSelectedPartSpecText(UpperPart upper)
    {
        _specTexts[(int)SpecType.AP].text = $"{upper.upperSO.armor}";
        _specTexts[(int)SpecType.Weight].text = $"{upper.upperSO.weight}";

        _specTexts[(int)SpecType.AttackMain].text = $"{upper.Primary.WeaponSO.atk}";
        _specTexts[(int)SpecType.AttackSub].text = $"{upper.Secondary.WeaponSO.atk}";
        _specTexts[(int)SpecType.ReloadSub].text = $"{upper.Secondary.WeaponSO.coolDownTime}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{upper.upperSO.smoothRotation}";
    }
}
