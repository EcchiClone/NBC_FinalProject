using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_TitleSceneButtons : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button optionBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(OnClickStartButton);
        optionBtn.onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_SettingsPopup>());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Managers.UI.ClosePopupUI();
    }

    public void OnClickStartButton()
    {
        if(!Managers.GameManager.gameData.tutorialClear)
            Managers.Scene.LoadScene(Scenes.TutorialScene);
        else
            Managers.Scene.LoadScene(Scenes.MainScene);
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
