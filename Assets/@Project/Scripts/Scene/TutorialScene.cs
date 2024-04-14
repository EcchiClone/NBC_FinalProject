using UnityEngine;

public class TutorialScene : BaseScene
{
    private bool _isTutorialClear = false;

    public override void Init()
    {
        Scenes = Define.Scenes.Tutorial;

        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();

        Managers.Tutorial.TutorialStart();
        Managers.Tutorial.OnTutorialClear += () => _isTutorialClear = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !_isTutorialClear)
            Managers.Tutorial.OnTutorialSkipPopup();
    }

    public override void Clear()
    {
        Managers.Clear();
    }
}
