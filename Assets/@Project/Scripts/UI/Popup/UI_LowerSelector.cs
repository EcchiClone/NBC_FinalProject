using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LowerSelector : UI_Selector
{
    [SerializeField] GameObject _nextGroup;
    [SerializeField] TextMeshProUGUI[] _specTexts;
    [SerializeField] TextMeshProUGUI[] _nextSpecTexts;    

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

        int createUI = Managers.Module.LowerCount;
        _changeBtns = new UI_ChangeButton[createUI];
        for (int i = 0; i < createUI; i++)
        {
            _changeBtns[i] = Managers.UI.ShowItemUI<UI_LowerChangeBtn>(_contents);
            _changeBtns[i].SetParentUI(this);
        }

        Managers.Module.OnLowerChange += UpdateSelectedPartSpecText;
        ResetText();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.BackToSelector).onClick.AddListener(BackToSelector);
    }

    public void ResetText()
    {
        int partID = Managers.Module.GetPartOfIndex<LowerPart>(Managers.GameManager.PartIndex_Lower).ID;
        PartData currentPartData = Managers.Data.GetPartData(partID);

        UpdateSelectedPartSpecText(currentPartData);
    }

    private void BackToSelector()
    {
        Managers.ActionManager.CallUndoMenuCam(Define.CamType.Lower);

        ResetText();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void UpdateSelectedPartSpecText(PartData lowerData)
    {
        _nextGroup.SetActive(false);
        _specTexts[(int)SpecType.AP].text = $"{lowerData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{lowerData.Weight}";

        _specTexts[(int)SpecType.MoveSpeed].text = $"{lowerData.Speed}";
        _specTexts[(int)SpecType.JumpPower].text = $"{lowerData.JumpPower}";
        _specTexts[(int)SpecType.BoostPower].text = $"{lowerData.BoosterPower}";
    }

    public void DisPlayNextPartSpecText(PartData nextLowerData)
    {
        _nextGroup.SetActive(true);
        _nextSpecTexts[(int)SpecType.AP].text = $"{nextLowerData.Armor}";
        _nextSpecTexts[(int)SpecType.Weight].text = $"{nextLowerData.Weight}";

        _nextSpecTexts[(int)SpecType.MoveSpeed].text = $"{nextLowerData.Speed}";
        _nextSpecTexts[(int)SpecType.JumpPower].text = $"{nextLowerData.JumpPower}";
        _nextSpecTexts[(int)SpecType.BoostPower].text = $"{nextLowerData.BoosterPower}";
    }
}
