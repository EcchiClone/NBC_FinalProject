using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class UI_LowerChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfLowerPart;
        ++IndexOfLowerPart;

        if (_currentIndex == Managers.GameManager.PartIndex_Lower)
            _equip.SetActive(true);

        GetCurrentPartData<LowerPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();
        Managers.ActionManager.OnLowerEquip += EquipPart;
    }

    private void EquipPart(int index)
    {
        if (_currentIndex != index)
            _equip.SetActive(false);
        else
            _equip.SetActive(true);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (!currentData.IsUnlocked)
        {
            Managers.Module.CallInfoChange("Locked", $"{LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-PartsInfo", LocalizationSettings.SelectedLocale)}");
            return;
        }

        UI_LowerSelector selector = _parentUI as UI_LowerSelector;
        selector.DisPlayNextPartSpecText(currentData);

        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {
        Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Lower);
        Managers.Module.CallLowerPartChange(currentData);
        Managers.ActionManager.CallLowerEquip(_currentIndex);
    }
}
