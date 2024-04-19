using System;
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

    public static event Action OnPairPopup;

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Button_GameStart).onClick.AddListener(() => { OnPairPopup = null; Managers.Scene.LoadScene(Define.Scenes.DevScene); });
        GetButton((int)Buttons.Button_Tutorial).onClick.AddListener(() => { OnPairPopup = null; Managers.GameManager.TutorialClear = false; Managers.Scene.LoadScene(Define.Scenes.TutorialScene); });

        GetButton((int)Buttons.Button_Back).onClick.AddListener(BackToMain);        
    }
    private void BackToMain()
    {
        OnPairPopup?.Invoke();
        _previousPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Managers.UI.ShowPopupUI<UI_ModuleACStatusPopup>(isStack: false);
    }
}
