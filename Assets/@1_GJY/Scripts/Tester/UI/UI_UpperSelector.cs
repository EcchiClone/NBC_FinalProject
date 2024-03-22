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
        ResetText();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResetText()
    {
        UpperPart upper = Managers.Module.CurrentUpperPart;        

        UpdateSelectedPartSpecText(upper);
    }

    private void UpdateSelectedPartSpecText(UpperPart upper)
    {
        _nextGroup.SetActive(false);
        _specTexts[(int)SpecType.AP].text = $"{upper.upperSO.armor}";
        _specTexts[(int)SpecType.Weight].text = $"{upper.upperSO.weight}";

        _specTexts[(int)SpecType.AttackMain].text = $"{upper.Primary.WeaponSO.atk}";
        _specTexts[(int)SpecType.AttackSub].text = $"{upper.Secondary.WeaponSO.atk}";
        _specTexts[(int)SpecType.ReloadSub].text = $"{upper.Secondary.WeaponSO.coolDownTime}";
        _specTexts[(int)SpecType.RotateSpeed].text = $"{upper.upperSO.smoothRotation}";
    }

    public void DisPlayNextPartSpecText(UpperPart nextUpper)
    {
        _nextGroup.SetActive(true);
        _nextSpecTexts[(int)SpecType.AP].text = $"{nextUpper.upperSO.armor}";
        _nextSpecTexts[(int)SpecType.Weight].text = $"{nextUpper.upperSO.weight}";

        _nextSpecTexts[(int)SpecType.AttackMain].text = $"{nextUpper.Primary.WeaponSO.atk}";
        _nextSpecTexts[(int)SpecType.AttackSub].text = $"{nextUpper.Secondary.WeaponSO.atk}";
        _nextSpecTexts[(int)SpecType.ReloadSub].text = $"{nextUpper.Secondary.WeaponSO.coolDownTime}";
        _nextSpecTexts[(int)SpecType.RotateSpeed].text = $"{nextUpper.upperSO.smoothRotation}";
    }
}
