using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShoulderChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;

    public int currentIndex;
    public static int IndexOfArmPart = 0;

    private PartData _currentShoulderData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        int partID = Managers.Module.GetPartOfIndex<ShouldersPart>(currentIndex).ID;
        _currentShoulderData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);

        _partText.text = _currentShoulderData.Display_Name;
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
        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;
        if (selector.CurrentChangeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
            selector.DisplayNextPartSpecText_L(_currentShoulderData);
        else
            selector.DisplayNextPartSpecText_R(_currentShoulderData);

        Managers.Module.CallInfoChange(_currentShoulderData.Display_Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;

        if (selector.CurrentChangeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
        {
            Managers.Module.ChangePart(currentIndex, Define.ChangePartsType.Weapon_Shoulder_L);
            Managers.Module.CallLeftShoulderPartChange(_currentShoulderData);
        }
        else
        {
            Managers.Module.ChangePart(currentIndex, Define.ChangePartsType.Weapon_Shoulder_R);
            Managers.Module.CallRightShoulderPartChange(_currentShoulderData);
        }
    }
}
