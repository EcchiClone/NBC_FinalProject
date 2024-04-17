using UnityEngine;

public class StageScene : BaseScene
{
    private UI_Scene _scene;

    public override void Init()
    {
        Scenes = Define.Scenes.DevScene;

        Managers.Module.CreatePlayerModule();
        _scene = Managers.UI.ShowSceneUI<UI_HUD>();
        Managers.SpawnManager.Init();        
    }

    public override void Clear()
    {
        DestroyImmediate(_scene.gameObject);
        Managers.Clear();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            Managers.ActionManager.CallPlayerDead();
    }
}
