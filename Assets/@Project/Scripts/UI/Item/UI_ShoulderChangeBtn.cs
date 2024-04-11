using UnityEngine.EventSystems;

public class UI_ShoulderChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfShoulderPart;
        ++IndexOfShoulderPart;

        if (_currentIndex == Managers.GameManager.PartIndex_LeftShoulder)
            _equip.SetActive(true);

        GetCurrentPartData<ShouldersPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();

        Managers.ActionManager.OnShoulderModeChange += ChangeMode;
        Managers.ActionManager.OnShoulderPartChange += ChangePart;        
    }

    private void ChangePart(int index)
    {
        if (_currentIndex != index)
            _equip.SetActive(false);
        else
            _equip.SetActive(true);
    }

    private void ChangeMode(UI_ShoulderSelector.ChangeShoulderMode changeMode)
    {
        if (changeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
        {
            Managers.ActionManager.CallSelectorCam(Define.CamType.Shoulder_Left);
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Shoulder_Right);
            if (Managers.GameManager.PartIndex_LeftShoulder == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
        else
        {
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Shoulder_Left);
            Managers.ActionManager.CallSelectorCam(Define.CamType.Shoulder_Right);
            if (Managers.GameManager.PartIndex_LeftShoulder == _currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!_currentData.IsUnlocked)
        {
            Managers.Module.CallInfoChange("Locked", "잠금 해제 후 정보 확인 가능");
            return;
        }

        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;
        if (selector.CurrentChangeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
            selector.DisplayNextPartSpecText_L(_currentData);
        else
            selector.DisplayNextPartSpecText_R(_currentData);
        
        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {
        UI_ShoulderSelector selector = _parentUI as UI_ShoulderSelector;
        Managers.ActionManager.CallShoulderPartChange(_currentIndex);

        if (selector.CurrentChangeMode == UI_ShoulderSelector.ChangeShoulderMode.LeftShoulder)
        {
            Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Weapon_Shoulder_L);
            Managers.Module.CallLeftShoulderPartChange(_currentData);
        }
        else
        {
            Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Weapon_Shoulder_R);
            Managers.Module.CallRightShoulderPartChange(_currentData);
        }
    }
}
