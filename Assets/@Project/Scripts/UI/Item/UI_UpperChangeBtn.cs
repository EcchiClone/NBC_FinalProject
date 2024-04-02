using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpperChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI _partText;
    [SerializeField] GameObject _equip;

    public int currentIndex;
    public static int IndexOfUpperPart = 0;

    private PartData _currentUpperData;

    protected override void Init()
    {
        base.Init();

        currentIndex = IndexOfUpperPart;
        ++IndexOfUpperPart;

        if (currentIndex == Managers.Module.CurrentUpperIndex)
            _equip.SetActive(true);

        int partID = Managers.Module.GetPartOfIndex<UpperPart>(currentIndex).ID;
        _currentUpperData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);
        
        _partText.text = _currentUpperData.Display_Name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = _currentUpperData.Display_Name;
        string desc = _currentUpperData.Display_Description;

        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);
            Managers.Module.CallInfoChange(name, desc);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        UI_UpperSelector selector = _parentUI as UI_UpperSelector;
        selector.DisPlayNextPartSpecText(_currentUpperData);
        
        Managers.Module.CallInfoChange(name, desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {        
        Managers.Module.ChangePart(currentIndex, Define.ChangePartsType.Upper);
        Managers.Module.CallUpperPartChange(_currentUpperData);
    }
}
