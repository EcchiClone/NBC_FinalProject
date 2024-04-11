using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseTester : MonoBehaviour
{
    private void Start()
    {
        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();
    }
}
