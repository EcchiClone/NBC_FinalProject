using UnityEngine;
using static Define;

public class UI_WorldUICanvas : UI_Popup
{
    private UI_SettingsOnStagePopup _settings;

    enum Buttons
    {
        Button_Start,
        Button_Option,
        Button_Exit,
    }

    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Button_Start).onClick.AddListener(OnClickStartButton);
        GetButton((int)Buttons.Button_Option).onClick.AddListener(OnClickSettingsButton);
        GetButton((int)Buttons.Button_Exit).onClick.AddListener(OnClickExitButton);
    }

    public void OnClickStartButton()
    {
        if (!Managers.GameManager.gameData.tutorialClear)
            Managers.Scene.LoadScene(Scenes.TutorialScene);
        else
            Managers.Scene.LoadScene(Scenes.MainScene);
    }
    private void OnClickSettingsButton()
    {
        if (_settings == null)
        {
            _settings = Managers.UI.ShowPopupUI<UI_SettingsOnStagePopup>();
            _settings.SetPreviousPopup(this);
        }
        else
            _settings.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    #region ExitGame Method
#if UNITY_EDITOR
    public void OnClickExitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
#else
    public void OnClickExitButton()
    {
        Application.Quit();
    } 
#endif
    #endregion
}