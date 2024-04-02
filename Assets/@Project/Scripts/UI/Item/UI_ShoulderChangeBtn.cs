using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShoulderChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _partImage;
    [SerializeField] GameObject _equip;

    public int currentIndex;
    public static int IndexOfArmPart = 0;

    private PartData _currentShoulderData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        if (currentIndex == Managers.Module.CurrentLeftShoulderIndex)
            _equip.SetActive(true);

        int partID = Managers.Module.GetPartOfIndex<ShouldersPart>(currentIndex).ID;
        _currentShoulderData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);

        Managers.ActionManager.OnShoulderModeChange += ChangeMode;
        Managers.ActionManager.OnShoulderPartChange += ChangePart;

        // 주석 풀어야 됨
        //Sprite weaponSprite = Resources.Load<Sprite>(_currentShoulderData.Sprite_Path);
        //_partImage.sprite = weaponSprite;
    }

    private void ChangePart(int index)
    {
        if (currentIndex != index)
            _equip.SetActive(false);
        else
            _equip.SetActive(true);
    }

    private void ChangeMode(UI_ShoulderSelector.ChangeShoulderMode changeMode)
    {
        if (changeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
        {
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Shoulder_Right);
            if (Managers.Module.CurrentLeftShoulderIndex == currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
        else
        {
            Managers.ActionManager.CallSelectorCam(Define.CamType.Shoulder_Right);
            if (Managers.Module.CurrentRightShoulderIndex == currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = _currentShoulderData.Display_Name;
        string desc = _currentShoulderData.Display_Description;

        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);
            Managers.Module.CallInfoChange(name, desc);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;
        if (selector.CurrentChangeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
            selector.DisplayNextPartSpecText_L(_currentShoulderData);
        else
            selector.DisplayNextPartSpecText_R(_currentShoulderData);
        
        Managers.Module.CallInfoChange(name, desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;
        Managers.ActionManager.CallArmPartChange(currentIndex);

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
