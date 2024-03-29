using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ArmChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;

    public int currentIndex;
    public static int IndexOfArmPart = 0;

    private PartData _currentArmData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        int partID = Managers.Module.GetPartOfIndex<ArmsPart>(currentIndex).ID;
        _currentArmData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);

        _partText.text = _currentArmData.Display_Name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_parentUI._sidePopup == null)
        {
            UI_PartsStatus info = Managers.UI.ShowPopupUI<UI_PartsStatus>();
            _parentUI.SetSidePopup(info);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        if(selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
            selector.DisplayNextPartSpecText_L(_currentArmData);
        else
            selector.DisplayNextPartSpecText_R(_currentArmData);

        Managers.Module.CallInfoChange(_currentArmData.Display_Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;

        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.Module.ChangePart(currentIndex, Define.ChangePartsType.Weapon_Arm_L);
            Managers.Module.CallLeftArmPartChange(_currentArmData);
        }
        else
        {
            Managers.Module.ChangePart(currentIndex, Define.ChangePartsType.Weapon_Arm_R);
            Managers.Module.CallRightArmPartChange(_currentArmData);
        }        
    }
}
