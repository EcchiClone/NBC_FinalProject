using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_TitleSceneButtons : MonoBehaviour
{
    public void OnClickStartButton()
    {
        if(Managers.GameManager.gameData.tutorialClear)
            Managers.Scene.LoadScene(Scenes.Tutorial);
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
