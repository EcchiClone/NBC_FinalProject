using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitTester : MonoBehaviour
{
    private void Awake()
    {
        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();


    }
}
