using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorSceneTester : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _currentSceneCam;

    private void Awake()
    {
        Managers.Module.CreateSelectorModule();
        UI_MainMenuPopup mainUI = Managers.UI.ShowPopupUI<UI_MainMenuPopup>();
        mainUI.BindCamAction(CamChange);
    }

    public void CamChange()
    {
        bool active = !_currentSceneCam.gameObject.activeSelf;
        _currentSceneCam.gameObject.SetActive(active);
    }
}
