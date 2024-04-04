using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UI_MainMenuPopup : UI_Popup
{
    private UI_SelectorMenu _selector;
    private UI_Achievement _achievement;
    
    enum Buttons
    {
        GameStart_Btn,
        Module_Select_Btn,
        Perk_Btn,
        Achievement_Btn,
        Settings_Btn,
        Exit_Btn,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));        

        GetButton((int)Buttons.GameStart_Btn).onClick.AddListener(() => SceneManager.LoadScene(1));
        GetButton((int)Buttons.Module_Select_Btn).onClick.AddListener(OpenModuleSelector);
        //GetButton((int)Buttons.Perk_Btn).onClick.AddListener();
        GetButton((int)Buttons.Achievement_Btn).onClick.AddListener(OpenAchievement);
        //GetButton((int)Buttons.Settings_Btn).onClick.AddListener();
        GetButton((int)Buttons.Exit_Btn).onClick.AddListener(ExitGame);
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
    private void OpenAchievement()
    {
        if (_selector == null)
        {
            _achievement = Managers.UI.ShowPopupUI<UI_Achievement>(); // Set on scene about Achievement UI
            _achievement.SetPreviousPopup(this);    // Set prev value(this(MainMenu))
           
        }
        else
            _achievement.gameObject.SetActive(true); // Show UI

        gameObject.SetActive(false); // Disable this(MainMenu)
    }

    #region ExitGame Method
#if UNITY_EDITOR
    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
#else
    private void ExitGame()
    {
        Application.Quit();
    } 
#endif
    #endregion

}