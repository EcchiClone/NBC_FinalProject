using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpperChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;

    public int currentIndex;
    public static int IndexOfUpperPart = 0;

    private UpperPartsSO _currentSO;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfUpperPart;
        ++IndexOfUpperPart;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Managers.Module.ChangeUpperPart(currentIndex));        

        _currentSO = Managers.Module.GetPartOfIndex<UpperPart>(currentIndex).upperSO;
        _partText.text = _currentSO.display_Name;        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_parentUI._sidePopup == null)
        {
            UI_PartsStatus info = Managers.UI.ShowPopupUI<UI_PartsStatus>();
            _parentUI.SetSidePopup(info);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        Managers.Module.CallInfoChange(_currentSO.display_Description);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }
}
