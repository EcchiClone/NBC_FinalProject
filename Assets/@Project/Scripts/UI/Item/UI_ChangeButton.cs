using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
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

    public PartData currentData;

    protected int _currentIndex;

    protected string _displayName;
    protected string _displayDesc;

    private bool _isUnlockChecked = false;

    private UnityAction _changePartAction;

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
        currentData = Managers.Data.GetPartData(partID);

        _displayName = currentData.Display_Name;
        if (LocalizationSettings.SelectedLocale.name == "Korean (ko)")
            _displayDesc = currentData.Display_Description_KO;
        else if(LocalizationSettings.SelectedLocale.name == "English (en)")
            _displayDesc = currentData.Display_Description_EN;


        CheckUnlockedPart();
    }

    public virtual void GetCurrentPartData() { }    

    public void CheckUnlockedPart()
    {
        if (LocalizationSettings.SelectedLocale.name == "Korean (ko)")
            _displayDesc = currentData.Display_Description_KO;
        else if (LocalizationSettings.SelectedLocale.name == "English (en)")
            _displayDesc = currentData.Display_Description_EN;

        if (_isUnlockChecked)
            return;        

        if (!currentData.IsUnlocked)
        {
            if (currentData.PointUnlock)
                _lockText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-ChangeBtn_Point", LocalizationSettings.SelectedLocale);
            else
                _lockText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-ChangeBtn_Reward", LocalizationSettings.SelectedLocale);            
        }
        else
        {
            _unlock.SetActive(false);
            _partImage.color = Color.white;
            _isUnlockChecked = true;
            if (_changePartAction != null)
            {
                Button button = GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(_changePartAction);
            }                
        }
    }

    protected void AddListenerToBtn(UnityAction action)
    {
        Button button = GetComponent<Button>();
        _changePartAction = action;

        if (!currentData.IsUnlocked)
        {
            if (currentData.PointUnlock)
                button.onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_UnlockPartPopup>().AlertTextUpdate(this));
            return;
        }
        button.onClick.AddListener(action);        
    }

    protected void LoadPartImage()
    {
        Sprite weaponSprite = Resources.Load<Sprite>(currentData.Sprite_Path);
        _partImage.sprite = weaponSprite;
    }
}
