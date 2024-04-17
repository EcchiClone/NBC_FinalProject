using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChangeButton : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _partImage;
    [SerializeField] protected TextMeshProUGUI _lockText;
    [SerializeField] protected GameObject _unlock;
    [SerializeField] protected GameObject _equip;

    public static int IndexOfLowerPart = 0;
    public static int IndexOfUpperPart = 0;
    public static int IndexOfArmPart = 0;
    public static int IndexOfShoulderPart = 0;

    protected PartData _currentData;
    protected int _currentIndex;

    protected string _displayName;
    protected string _displayDesc;

    private bool _isUnlockChecked = false;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);                        
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

        CheckUnlockedPart();
    }

    public void CheckUnlockedPart()
    {
        if (_isUnlockChecked)
            return;        

        if (!_currentData.IsUnlocked)
        {
            if (_currentData.PointUnlock)
                _lockText.text = "업적 포인트로 잠금 해제";
            else
                _lockText.text = "업적 보상으로 잠금 해제";
        }
        else
        {
            _unlock.SetActive(false);
            _partImage.color = Color.white;
            _isUnlockChecked = true;
        }
    }

    protected void AddListenerToBtn(UnityAction action)
    {
        Button button = GetComponent<Button>();

        if (!_currentData.IsUnlocked)
        {
            if (_currentData.PointUnlock)
                button.onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_UnlockPartPopup>().AlertTextUpdate(_currentData.Point));
            return;
        }
        button.onClick.AddListener(action);
    }

    protected void LoadPartImage()
    {
        Sprite weaponSprite = Resources.Load<Sprite>(_currentData.Sprite_Path);
        _partImage.sprite = weaponSprite;
    }
}
