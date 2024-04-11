using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnlockPartPopup : UI_Popup
{
    enum Texts
    {
        Alert_Text,
    }

    enum Buttons
    {
        Yes_Btn,
        No_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindTMP(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.No_Btn).onClick.AddListener(ClosePopupUI);
    }

    public void AlertTextUpdate(int point)
    {
        GetTMP((int)Texts.Alert_Text).text = $"업적포인트 {point} 포인트를 사용하여\n파츠 잠금을 해제하시겠습니까?";
    }
}
