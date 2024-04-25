using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class UI_UpperChangeBtn : UI_ChangeButton
{
    protected override void Init()
    {
        base.Init();

        _currentIndex = IndexOfUpperPart;
        ++IndexOfUpperPart;

        if (_currentIndex == Managers.GameManager.PartIndex_Upper)
            _equip.SetActive(true);

        GetCurrentPartData<UpperPart>();
        AddListenerToBtn(ChangePart);
        LoadPartImage();
        Managers.ActionManager.OnUpperEquip += EquipPart;
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

        UI_UpperSelector selector = _parentUI as UI_UpperSelector;
        selector.DisPlayNextPartSpecText(currentData);
        
        Managers.Module.CallInfoChange(_displayName, _displayDesc);
    }

    private void ChangePart()
    {        
        Managers.Module.ChangePart(_currentIndex, Define.Parts_Location.Upper);
        Managers.Module.CallUpperPartChange(currentData);
        Managers.ActionManager.CallUpperEquip(_currentIndex);
    }
}
