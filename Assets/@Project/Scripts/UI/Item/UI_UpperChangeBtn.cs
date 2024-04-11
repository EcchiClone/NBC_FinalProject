using UnityEngine.EventSystems;

public class UI_UpperChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfUpperPart;
        ++IndexOfUpperPart;

        if (_currentIndex == Managers.GameManager.PartIndex_Upper)
            _equip.SetActive(true);

        GetCurrentPartData<UpperPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!_currentData.IsUnlocked)
        {
            Managers.Module.CallInfoChange("Locked", "잠금 해제 후 정보 확인 가능");
            return;
        }

        UI_UpperSelector selector = _parentUI as UI_UpperSelector;
        selector.DisPlayNextPartSpecText(_currentData);
        
        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {        
        Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Upper);
        Managers.Module.CallUpperPartChange(_currentData);
    }
}
