using UnityEngine.EventSystems;

public class UI_ArmChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        if (_currentIndex == Managers.GameManager.PartIndex_LeftArm)
            _equip.SetActive(true);

        GetCurrentPartData<ArmsPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();

        Managers.ActionManager.OnArmModeChange += ChangeMode;
        Managers.ActionManager.OnArmEquip += EquipPart;
    }

    private void EquipPart(int index)
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
            if (Managers.GameManager.PartIndex_LeftArm == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
        else
        {
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Arm_Left);
            Managers.ActionManager.CallSelectorCam(Define.CamType.Arm_Right);
            if (Managers.GameManager.PartIndex_RightArm == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!currentData.IsUnlocked)
        {
            Managers.Module.CallInfoChange("Locked", "잠금 해제 후 정보 확인 가능");
            return;
        }

        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
            selector.DisplayNextPartSpecText_L(currentData);
        else
            selector.DisplayNextPartSpecText_R(currentData);

        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        Managers.ActionManager.CallArmEquip(_currentIndex);

        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Weapon_Arm_L);
            Managers.Module.CallLeftArmPartChange(currentData);
        }
        else
        {
            Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Weapon_Arm_R);
            Managers.Module.CallRightArmPartChange(currentData);
        }
    }
}
