using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_StageSelectPopup : UI_Popup
{
    enum Buttons
    {
        Button_GameStart,
        Button_Tutorial,
        Button_Back,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Button_GameStart).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.DevScene));
        GetButton((int)Buttons.Button_Tutorial).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.Tutorial));

        GetButton((int)Buttons.Button_Back).onClick.AddListener(BackToMain);
    }
    private void BackToMain()
    {
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
