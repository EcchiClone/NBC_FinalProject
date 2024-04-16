using UnityEngine;

public class TutorialScene : BaseScene
{
    [SerializeField] TutorialDissolveController _linkedWall;
    [SerializeField] GameObject _dontGoBackWall;
    [SerializeField] GameObject _dummy;

    public override void Init()
    {
        Scenes = Define.Scenes.Tutorial;

        Managers.Module.CreatePlayerModule();
        Managers.UI.ShowSceneUI<UI_HUD>();
        Managers.Tutorial.TutorialStart();

        Managers.Tutorial.OnDisableLinkedWall += _linkedWall.Dissolve;
        Managers.Tutorial.OnEnableDontGoBackWall += () => _dontGoBackWall.SetActive(true);
        Managers.Tutorial.OnEnableDummy += () => _dummy.SetActive(true);
    }    

    public override void Clear()
    {
        Managers.Clear();
    }
}
