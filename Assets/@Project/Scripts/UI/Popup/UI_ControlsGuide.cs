using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlsGuide : UI_Popup
{
    enum Buttons
    {
        Button_Back,
        Background,
    }
    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_Back).onClick.AddListener(() => ClosePopupUI());
        GetButton((int)Buttons.Background).onClick.AddListener(() => ClosePopupUI());
    }
}
