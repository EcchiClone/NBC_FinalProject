using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LowerChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;
    [SerializeField] GameObject _equip;

    public int currentIndex;
    public static int IndexOfLowerPart = 0;

    private PartData _currentLowerData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfLowerPart;
        ++IndexOfLowerPart;

        if (currentIndex == Managers.Module.CurrentLowerIndex)
            _equip.SetActive(true);

        int partID = Managers.Module.GetPartOfIndex<LowerPart>(currentIndex).ID;
        _currentLowerData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);        

        _partText.text = _currentLowerData.Display_Name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = _currentLowerData.Display_Name;
        string desc = _currentLowerData.Display_Description;

        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);
            Managers.Module.CallInfoChange(name, desc);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        UI_LowerSelector selector = _parentUI as UI_LowerSelector;
        selector.DisPlayNextPartSpecText(_currentLowerData);
        
        Managers.Module.CallInfoChange(name, desc);        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        Managers.Module.ChangePart(currentIndex, Define.PartsType.Lower);
        Managers.Module.CallLowerPartChange(_currentLowerData);
    }
}
