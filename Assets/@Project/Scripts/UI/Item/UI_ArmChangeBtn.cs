using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ArmChangeBtn : UI_Item, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _partImage;
    [SerializeField] GameObject _equip;

    public int currentIndex;
    public static int IndexOfArmPart = 0;

    private PartData _currentArmData;

    protected override void Init()
    {
        base.Init();        

        currentIndex = IndexOfArmPart;
        ++IndexOfArmPart;

        if (currentIndex == Managers.Module.CurrentLeftArmIndex)
            _equip.SetActive(true);

        int partID = Managers.Module.GetPartOfIndex<ArmsPart>(currentIndex).ID;
        _currentArmData = Managers.Data.GetPartData(partID);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangePart);

        Managers.ActionManager.OnArmModeChange += ChangeMode;
        Managers.ActionManager.OnArmPartChange += ChangePart;

        Sprite weaponSprite = Resources.Load<Sprite>(_currentArmData.Sprite_Path);
        _partImage.sprite = weaponSprite;
    }

    private void ChangePart(int index)
    {
        if (currentIndex != index)
            _equip.SetActive(false);
        else
            _equip.SetActive(true);
    }

    private void ChangeMode(UI_ArmSelector.ChangeArmMode changeMode)
    {
        if (changeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.ActionManager.CallUndoMenuCam(Define.CamType.Arm_Right);
            if (Managers.Module.CurrentLeftArmIndex == currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }
        else
        {
            Managers.ActionManager.CallSelectorCam(Define.CamType.Arm_Right);
            if (Managers.Module.CurrentRightArmIndex == currentIndex)
                _equip.SetActive(true);
            else
                _equip.SetActive(false);
        }            
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string name = _currentArmData.Display_Name;
        string desc = _currentArmData.Display_Description;

        if (_parentUI._sidePopup == null)
        {
            UI_PartsInfo info = Managers.UI.ShowPopupUI<UI_PartsInfo>();
            _parentUI.SetSidePopup(info);
            Managers.Module.CallInfoChange(name, desc);
            return;
        }

        _parentUI._sidePopup.gameObject.SetActive(true);
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        if(selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
            selector.DisplayNextPartSpecText_L(_currentArmData);
        else
            selector.DisplayNextPartSpecText_R(_currentArmData);
        
        Managers.Module.CallInfoChange(name, desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentUI._sidePopup.gameObject.SetActive(false);
    }

    private void ChangePart()
    {
        UI_ArmSelector selector = _parentUI as UI_ArmSelector;
        Managers.ActionManager.CallArmPartChange(currentIndex);        

        if (selector.CurrentChangeMode == UI_ArmSelector.ChangeArmMode.LeftArm)
        {
            Managers.Module.ChangePart(currentIndex, Define.PartsType.Weapon_Arm_L);
            Managers.Module.CallLeftArmPartChange(_currentArmData);
        }
        else
        {
            Managers.Module.ChangePart(currentIndex, Define.PartsType.Weapon_Arm_R);
            Managers.Module.CallRightArmPartChange(_currentArmData);
        }        
    }
}
