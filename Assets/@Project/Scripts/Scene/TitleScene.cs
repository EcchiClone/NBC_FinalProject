using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TitleScene : BaseScene
{
    public override void Init()
    {
        Scenes = Define.Scenes.TitleScene;
    }

    public override void Clear()
    {
        Managers.Clear();
    }
}
