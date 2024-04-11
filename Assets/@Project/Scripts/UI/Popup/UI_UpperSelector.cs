using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpperSelector : UI_Selector
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
        RotateSpeed,
        BoostCapacity,
        HoverPower,        
    }    

    protected override void Init()
    {
        base.Init();

        int createUI = Managers.Module.UpperCount;
        _changeBtns = new UI_ChangeButton[createUI];
        for (int i = 0; i < createUI; i++)
        {
            _changeBtns[i] = Managers.UI.ShowItemUI<UI_UpperChangeBtn>(_contents);
            _changeBtns[i].SetParentUI(this);
        }

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
    
    private void ResetText()
    {        
        int partID = Managers.Module.GetPartOfIndex<UpperPart>(Managers.GameManager.PartIndex_Upper).ID;
        PartData currentPartData = Managers.Data.GetPartData(partID);

        UpdateSelectedPartSpecText(currentPartData);
    }

    private void UpdateSelectedPartSpecText(PartData upperData)
    {
        _nextGroup.SetActive(false);
        _specTexts[(int)SpecType.AP].text = $"{upperData.Armor}";
        _specTexts[(int)SpecType.Weight].text = $"{upperData.Weight}";

        _specTexts[(int)SpecType.RotateSpeed].text = $"{upperData.SmoothRotation}";
        _specTexts[(int)SpecType.BoostCapacity].text = $"{upperData.BoosterGauge}";
        _specTexts[(int)SpecType.HoverPower].text = $"{upperData.Hovering}";
    }

    public void DisPlayNextPartSpecText(PartData nextUpperData)
    {
        _nextGroup.SetActive(true);
        _nextSpecTexts[(int)SpecType.AP].text = $"{nextUpperData.Armor}";
        _nextSpecTexts[(int)SpecType.Weight].text = $"{nextUpperData.Weight}";

        _nextSpecTexts[(int)SpecType.RotateSpeed].text = $"{nextUpperData.SmoothRotation}";
        _nextSpecTexts[(int)SpecType.BoostCapacity].text = $"{nextUpperData.BoosterGauge}";
        _nextSpecTexts[(int)SpecType.HoverPower].text = $"{nextUpperData.Hovering}";
    }
}
