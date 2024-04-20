using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TitleScene : BaseScene
{
    public override void Init()
    {
        base.Init();
        // 저장된 해상도 사용
        int width = PlayerPrefs.GetInt("ResolutionWidth", 0);
        int height = PlayerPrefs.GetInt("ResolutionHeight", 0);
        if (!(width == 0 || height == 0))
        {
            FullScreenMode mode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", (int)FullScreenMode.FullScreenWindow);
            Screen.SetResolution(width, height, mode);
        }

        Scenes = Define.Scenes.TitleScene;
    }

    public override void Clear()
    {
        Managers.Clear();
    }
}
