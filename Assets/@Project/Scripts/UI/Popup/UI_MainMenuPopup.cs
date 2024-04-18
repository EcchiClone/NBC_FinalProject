using UnityEngine;

public class UI_MainMenuPopup : UI_Popup
{
    private UI_SelectorMenu _selector;
    private UI_Achievement _achievement;
    private UI_SettingsPopup _settings;
    private UI_StageSelectPopup _stageSelect;

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

        GetButton((int)Buttons.GameStart_Btn).onClick.AddListener(OpenStageSelect);
        GetButton((int)Buttons.Perk_Btn).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.PerkViewerScene));

        GetButton((int)Buttons.Module_Select_Btn).onClick.AddListener(OpenModuleSelector);        
        GetButton((int)Buttons.Achievement_Btn).onClick.AddListener(OpenAchievement);
        GetButton((int)Buttons.Settings_Btn).onClick.AddListener(OpenSettings);
        GetButton((int)Buttons.Exit_Btn).onClick.AddListener(ExitGame);
    }

    private void OpenStageSelect()
    {
        if (_stageSelect == null)
        {
            _stageSelect = Managers.UI.ShowPopupUI<UI_StageSelectPopup>();
            _stageSelect.SetPreviousPopup(this);
        }
        else
            _stageSelect.gameObject.SetActive(true);

        gameObject.SetActive(false);
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
        if (_achievement == null)
        {
            _achievement = Managers.UI.ShowPopupUI<UI_Achievement>(); // Set on scene about Achievement UI
            _achievement.SetPreviousPopup(this);    // Set prev value(this(MainMenu))            
        }
        else
            _achievement.gameObject.SetActive(true); // Show UI

        gameObject.SetActive(false); // Disable this(MainMenu)
    }
    private void OpenSettings()
    {
        if (_settings == null)
        {
            _settings = Managers.UI.ShowPopupUI<UI_SettingsPopup>();
            _settings.SetPreviousPopup(this);
        }
        else
            _settings.gameObject.SetActive(true);

        gameObject.SetActive(false);
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