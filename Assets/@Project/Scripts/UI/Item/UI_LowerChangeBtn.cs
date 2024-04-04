using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LowerChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfLowerPart;
        ++IndexOfLowerPart;

        if (_currentIndex == Managers.Module.CurrentLowerIndex)
            _equip.SetActive(true);

        GetCurrentPartData<LowerPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        UI_LowerSelector selector = _parentUI as UI_LowerSelector;
        selector.DisPlayNextPartSpecText(_currentData);

        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {
        Managers.Module.ChangePart(_currentIndex, Define.PartsType.Lower);
        Managers.Module.CallLowerPartChange(_currentData);
    }
}
