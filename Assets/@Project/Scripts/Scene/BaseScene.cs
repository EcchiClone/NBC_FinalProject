using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scenes Scenes { get; protected set; } = Define.Scenes.Unknown;

    private UI_ControlsGuide _keyGuidePanel;

    private void Awake()
    {        
        Init();
    }

    public virtual void Init() 
    {
        Managers.Input.OnInputKeyDown += KeyGuidPanel;
        Util.IsCleared = false;
    }

    private void KeyGuidPanel()
    {        
        if(Input.GetKeyDown(KeyCode.BackQuote)) 
        {
            if (_keyGuidePanel == null)
                _keyGuidePanel = Managers.UI.ShowPopupUI<UI_ControlsGuide>();
            else
                _keyGuidePanel.ClosePopupUI();
        }
    }

    public abstract void Clear();
}
