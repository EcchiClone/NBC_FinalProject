using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LowerChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;
    
    public int currentIndex;
    public static int IndexOfLowerPart = 0;

    private PartData _currentLowerData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfLowerPart;
        ++IndexOfLowerPart;

        int partID = Managers.Module.GetPartOfIndex<LowerPart>(currentIndex).ID;
        _currentLowerData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);        

        _partText.text = _currentLowerData.Display_Name;
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
        UI_LowerSelector selector = _parentUI as UI_LowerSelector;
        selector.DisPlayNextPartSpecText(_currentLowerData);
        Managers.Module.CallInfoChange(_currentLowerData.Display_Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        Managers.Module.ChangeLowerPart(currentIndex);
        Managers.Module.CallLowerPartChange(_currentLowerData);
    }
}
