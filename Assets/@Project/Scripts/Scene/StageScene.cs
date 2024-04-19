using UnityEngine;

public class StageScene : BaseScene
{
    private UI_Scene _scene;
    StageController _stageController;

    public override void Init()
    {
        base.Init();

        Scenes = Define.Scenes.DevScene;

        if (!Managers.GameManager.FirstPlayDone)
        {
            //Managers.UI.ShowPopupUI
            Managers.GameManager.FirstPlayDone = true;
        }

        Managers.Module.CreatePlayerModule();
        _scene = Managers.UI.ShowSceneUI<UI_HUD>();
        Managers.SpawnManager.Init();
    }

    private void Start()
    {
        _stageController = GetComponent<StageController>();
        _stageController.GameStart();
    }

    public override void Clear()
    {
        DestroyImmediate(_scene.gameObject);
        Managers.Clear();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            Managers.ActionManager.CallPlayerDead();
    }
#endif
}
