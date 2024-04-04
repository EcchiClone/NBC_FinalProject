using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpperChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfUpperPart;
        ++IndexOfUpperPart;

        if (_currentIndex == Managers.Module.CurrentUpperIndex)
            _equip.SetActive(true);

        GetCurrentPartData<UpperPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        UI_UpperSelector selector = _parentUI as UI_UpperSelector;
        selector.DisPlayNextPartSpecText(_currentData);
        
        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {        
        Managers.Module.ChangePart(_currentIndex, Define.PartsType.Upper);
        Managers.Module.CallUpperPartChange(_currentData);
    }
}
