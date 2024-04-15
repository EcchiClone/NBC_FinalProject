using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene : BaseScene
{
    public override void Init()
    {
        Scenes = Define.Scenes.DevScene;

        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();
    }

    public override void Clear()
    {
        Managers.Clear();
    }
}
