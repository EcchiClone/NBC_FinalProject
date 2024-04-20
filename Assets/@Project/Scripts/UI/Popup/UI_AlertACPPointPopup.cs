using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AlertACPPointPopup : UI_Popup
{
    enum Buttons
    {
        Yes_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Yes_Btn).onClick.AddListener(() => ClosePopupUI());
    }
}
