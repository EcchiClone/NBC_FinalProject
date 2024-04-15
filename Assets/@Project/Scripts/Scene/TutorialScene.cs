using UnityEngine;

public class TutorialScene : BaseScene
{
    public override void Init()
    {
        Scenes = Define.Scenes.Tutorial;

        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();
        Managers.Tutorial.TutorialStart();
    }

    public override void Clear()
    {
        Managers.Clear();
    }
}
