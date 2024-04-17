using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PausePopup : UI_Popup
{
    private UI_SettingsOnStagePopup _settings;

    enum Buttons
    {
        Button_Resume,
        Button_Restart,
        Button_Settings,
        Button_Exit,
        Background,
    }
    private void OnEnable()
    {
        // 마우스 살리기
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    protected override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_Resume).onClick.AddListener(Resume);
        GetButton((int)Buttons.Button_Restart).onClick.AddListener(Restart);
        GetButton((int)Buttons.Button_Settings).onClick.AddListener(Settings);
        GetButton((int)Buttons.Button_Exit).onClick.AddListener(Exit);
        GetButton((int)Buttons.Background).onClick.AddListener(Resume);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        // 마우스 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 팝업 비활성화
        gameObject.SetActive(false);
    }
    private void Restart()
    {
        Time.timeScale = 1;
        // 씬 다시 로드        
        Managers.Scene.LoadScene(Scenes.DevScene);
    }
    private void Settings()
    {
        // 임시로 메인메뉴의 settings 패널 사용
        if (_settings == null)
        {
            _settings = Managers.UI.ShowPopupUI<UI_SettingsOnStagePopup>();
            _settings.SetPreviousPopup(this);
        }
        else
            _settings.gameObject.SetActive(true);
    }
    private void Exit()
    {
        Time.timeScale = 1;
        // 메인메뉴로
        Managers.Scene.LoadScene(Scenes.MainScene);
    }
}
