using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class UI_UnlockPartPopup : UI_Popup
{
    enum Texts
    {
        Alert_Text,
        AchievementPoint_Text,
    }

    enum Buttons
    {
        Yes_Btn,
        No_Btn,
    }

    private UI_ChangeButton partButton;

    protected override void Init()
    {
        base.Init();

        BindTMP(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.No_Btn).onClick.AddListener(ClosePopupUI);
        GetButton((int)Buttons.Yes_Btn).onClick.AddListener(UnlockPart);
    }

    public void AlertTextUpdate(UI_ChangeButton button)
    {
        partButton = button;
        //GetTMP((int)Texts.Alert_Text).text = $"업적포인트 [<color=green>{partButton.currentData.Point}</color>] 포인트를 사용하여\n파츠 잠금을 해제하시겠습니까?";
        GetTMP((int)Texts.Alert_Text).text = $"{LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-UnlockPartPopup_Alert1", LocalizationSettings.SelectedLocale)}" +
            $" [<color=green>{partButton.currentData.Point}</color>] " +
            $"{LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-UnlockPartPopup_Alert2", LocalizationSettings.SelectedLocale)}";

        GetTMP((int)Texts.AchievementPoint_Text).text = $"{LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Module Table", "UI-UnlockPartPopup_Point", LocalizationSettings.SelectedLocale)}" +
            $" {Managers.GameManager.AchievementPoint}";
    }

    private void UnlockPart()
    {
        if (Managers.GameManager.AchievementPoint < partButton.currentData.Point)
        {
            ClosePopupUI();
            Managers.UI.ShowPopupUI<UI_AlertACPPointPopup>();            
            return;
        }

        Managers.GameManager.ReceivePartID(partButton.currentData.Dev_ID);
        Managers.GameManager.AchievementPoint -= partButton.currentData.Point;
        partButton.CheckUnlockedPart();
        ClosePopupUI();
    }
}
