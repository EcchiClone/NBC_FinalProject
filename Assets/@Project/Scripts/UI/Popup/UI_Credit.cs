using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Credit : UI_Popup
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
        GetButton((int)Buttons.Button_Back).onClick.AddListener(BackToMain);
        GetButton((int)Buttons.Background).onClick.AddListener(BackToMain);
    }
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}