using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LowerSelector : UI_Popup
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
        MoveSpeed,
        JumpPower,
        BoostPower,
    }

    protected override void Init()
    {
        base.Init();

        int createUI = Managers.Module.LowerPartsCount;

        for (int i = 0; i < createUI; i++)
            Managers.UI.ShowItemUI<UI_LowerChangeBtn>(_contents).SetParentUI(this);

        Managers.Module.OnLowerChange += UpdateSelectedPartSpecText;
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
        LowerPart lower = Managers.Module.CurrentLowerPart;

        UpdateSelectedPartSpecText(lower);
    }

    private void UpdateSelectedPartSpecText(LowerPart lower)
    {
        _nextGroup.SetActive(false);
        _specTexts[(int)SpecType.AP].text = $"{lower.lowerSO.armor}";
        _specTexts[(int)SpecType.Weight].text = $"{lower.lowerSO.weight}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{lower.lowerSO.speed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{lower.lowerSO.jumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{lower.lowerSO.boosterPower}";        
    }

    public void DisPlayNextPartSpecText(LowerPart nextLower)
    {
        _nextGroup.SetActive(true);
        _nextSpecTexts[(int)SpecType.AP].text = $"{nextLower.lowerSO.armor}";
        _nextSpecTexts[(int)SpecType.Weight].text = $"{nextLower.lowerSO.weight}";

        _nextSpecTexts[(int)SpecType.MoveSpeed].text = $"{nextLower.lowerSO.speed}";
        _nextSpecTexts[(int)SpecType.JumpPower].text = $"{nextLower.lowerSO.jumpPower}";
        _nextSpecTexts[(int)SpecType.BoostPower].text = $"{nextLower.lowerSO.boosterPower}";
    }
}
