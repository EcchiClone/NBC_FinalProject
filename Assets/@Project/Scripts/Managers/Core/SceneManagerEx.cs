using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    public void LoadScene(Define.Scenes sceneType)
    {
        SceneManager.LoadScene(GetSceneName(sceneType));

        CurrentScene.Clear();
    }

    private string GetSceneName(Define.Scenes sceneType) => Enum.GetName(typeof(Define.Scenes), sceneType);

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
