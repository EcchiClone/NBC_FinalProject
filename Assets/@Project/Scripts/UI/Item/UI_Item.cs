using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : UI_Base
{
    protected UI_Popup _parentUI;

    protected override void Init() { }

    public void SetParentUI(UI_Popup popup)
    {
        _parentUI = popup;
    }
}
