using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public Define.Scenes CurrentScene { get; private set; }

    public void LoadScene(Define.Scenes sceneType)
    {
        SceneManager.LoadScene(GetSceneName(sceneType));
        
    }

    private string GetSceneName(Define.Scenes sceneType) => Enum.GetName(typeof(Define.Scenes), sceneType);
}
