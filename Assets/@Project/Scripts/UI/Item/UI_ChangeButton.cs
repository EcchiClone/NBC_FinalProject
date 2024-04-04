using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChangeButton : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _partImage;
    [SerializeField] protected GameObject _equip;

    protected PartData _currentData;
    protected int _currentIndex;

    protected static int IndexOfLowerPart = 0;
    protected static int IndexOfUpperPart = 0;
    protected static int IndexOfArmPart = 0;
    protected static int IndexOfShoulderPart = 0;

    protected string _displayName;
    protected string _displayDesc;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);
            Managers.Module.CallInfoChange(_displayName, _displayDesc);
            return;
        }
        _parentUI._sidePopup.gameObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    protected void GetCurrentPartData<T>() where T : BasePart
    {
        int partID = Managers.Module.GetPartOfIndex<T>(_currentIndex).ID;
        _currentData = Managers.Data.GetPartData(partID);
        _displayName = _currentData.Display_Name;
        _displayDesc = _currentData.Display_Description;
    }

    protected void AddListenerToBtn(UnityAction action)
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(action);
    }

    protected void LoadPartImage()
    {
        Sprite weaponSprite = Resources.Load<Sprite>(_currentData.Sprite_Path);
        _partImage.sprite = weaponSprite;
    }
}
