using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UI_MainMenuPopup : UI_Popup
{
    private UI_SelectorMenu _selector;

    enum Buttons
    {
        GameStart_Btn,
        Module_Select_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));        

        GetButton((int)Buttons.GameStart_Btn).onClick.AddListener(() => SceneManager.LoadScene(1));
        GetButton((int)Buttons.Module_Select_Btn).onClick.AddListener(OpenModuleSelector);
    }

    private void OpenModuleSelector()
    {
        if (_selector == null)
        {
            _selector = Managers.UI.ShowPopupUI<UI_SelectorMenu>();
            _selector.SetPreviousPopup(this);            
        }
        else
            _selector.gameObject.SetActive(true);

        Managers.ActionManager.CallSelectorCam(Define.CamType.Module);
        gameObject.SetActive(false);
    }
}