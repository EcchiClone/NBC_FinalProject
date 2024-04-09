using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ArmChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        if (_currentIndex == Managers.Module.CurrentLeftArmIndex)
            _equip.SetActive(true);

        GetCurrentPartData<ArmsPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();

        Managers.ActionManager.OnArmModeChange += ChangeMode;
        Managers.ActionManager.OnArmPartChange += ChangePart;
    }

    private void ChangePart(int index)
    {
        if (_currentIndex != index)
            _equip.SetActive(false);
        else
            _equip.SetActive(true);
    }

    private void ChangeMode(UI_ArmSelector.ChangeArmMode changeMode)
    {
        if (changeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.ActionManager.CallSelectorCam(Define.CamType.Arm_Left);
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Arm_Right);
            if (Managers.Module.CurrentLeftArmIndex == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
        else
        {
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Arm_Left);
            Managers.ActionManager.CallSelectorCam(Define.CamType.Arm_Right);
            if (Managers.Module.CurrentRightArmIndex == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
            selector.DisplayNextPartSpecText_L(_currentData);
        else
            selector.DisplayNextPartSpecText_R(_currentData);

        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        Managers.ActionManager.CallArmPartChange(_currentIndex);

        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.Module.ChangePart(_currentIndex, Define.PartsType.Weapon_Arm_L);
            Managers.Module.CallLeftArmPartChange(_currentData);
        }
        else
        {
            Managers.Module.ChangePart(_currentIndex, Define.PartsType.Weapon_Arm_R);
            Managers.Module.CallRightArmPartChange(_currentData);
        }
    }
}
