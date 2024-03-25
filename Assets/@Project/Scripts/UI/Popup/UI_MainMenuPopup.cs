using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UI_MainMenuPopup : UI_Popup
{
    private UI_SelectorMenu _selector;
    private UnityAction _camAction;

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

    public void BindCamAction(UnityAction camAction)
    {
        _camAction = camAction;
    }

    private void OpenModuleSelector()
    {
        if (_selector == null)
        {
            _selector = Managers.UI.ShowPopupUI<UI_SelectorMenu>();
            _selector.SetPreviousPopup(this);
            _selector.BindCamAction(_camAction);
        }
        else
            _selector.gameObject.SetActive(true);

        _camAction.Invoke();
        gameObject.SetActive(false);
    }
}