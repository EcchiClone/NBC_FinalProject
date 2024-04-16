using System;
using UnityEngine;

public class TutorialManager
{
    public event Action OnTutorialClear;
    public event Action OnScriptPhaseUpdate;
    public event Action OnDisableLinkedWall;
    public event Action OnEnableDontGoBackWall;
    public event Action OnEnableDummy;
    public event Action OnShootDummy;

    public Achievement CurrentMission { get; private set; }
    public bool IsMissioning { get; private set; }

    private UI_Popup _skipPopup;

    private readonly float BLIND_TIME = 2f;

    public void TutorialStart()
    {
        Managers.Input.OnInputKeyDown += TutorialKeyInput;

        UI_BlinderPopup blinder = Managers.UI.ShowPopupUI<UI_BlinderPopup>();
        blinder.DoOpenBlind(BLIND_TIME, () => Managers.UI.ShowPopupUI<UI_TutorialDialoguePopup>());
    }

    public void TutorialClear()
    {
        Managers.GameManager.TutorialClear = true;
        OnTutorialClear?.Invoke();

        Managers.Scene.LoadScene(Define.Scenes.MainScene);
    }

    public void OnTutorialSkipPopup()
    {
        if (_skipPopup != null)
            return;

        _skipPopup = Managers.UI.ShowPopupUI<UI_TutorialSkipPopup>();
    }

    public void GiveCurrentMission(int id)
    {        
        Achievement mission = Resources.Load<Achievement>(Managers.Data.GetTutorialData(id).MissionPath);
        CurrentMission = UnityEngine.Object.Instantiate(mission);

        // 극한의 하드코딩...
        if (id == 4)
            OnDisableLinkedWall?.Invoke();
        if (id == 5)
            OnEnableDontGoBackWall?.Invoke();
        if (id == 7)
            OnEnableDummy?.Invoke();
        if (id == 8)
            OnShootDummy?.Invoke();

        Managers.AchievementSystem.Register(CurrentMission);
        IsMissioning = true;
    }

    public void NextPhase()
    {
        OnScriptPhaseUpdate?.Invoke();
        IsMissioning = false;
    }

    private void TutorialKeyInput() // 극한의 하드코딩
    {
        if (Input.GetKeyDown(KeyCode.W))
            Managers.AchievementSystem.ReceiveReport("TUTO1", KeyCode.W, 1);
        if (Input.GetKeyDown(KeyCode.A))
            Managers.AchievementSystem.ReceiveReport("TUTO1", KeyCode.A, 1);
        if (Input.GetKeyDown(KeyCode.S))
            Managers.AchievementSystem.ReceiveReport("TUTO1", KeyCode.S, 1);
        if (Input.GetKeyDown(KeyCode.D))
            Managers.AchievementSystem.ReceiveReport("TUTO1", KeyCode.D, 1);
        if (Input.GetKeyDown(KeyCode.Space))
            Managers.AchievementSystem.ReceiveReport("TUTO2", KeyCode.Space, 1);
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKeyDown(KeyCode.LeftShift))
            Managers.AchievementSystem.ReceiveReport("TUTO3", KeyCode.LeftShift, 1);
        if (Managers.Module.CurrentModule.PlayerStateMachine.IsHovering == true && Input.GetKeyDown(KeyCode.Space))
            Managers.AchievementSystem.ReceiveReport("TUTO4", KeyCode.Space, 1);
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Managers.AchievementSystem.ReceiveReport("TUTO6", KeyCode.K, 1);
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Managers.AchievementSystem.ReceiveReport("TUTO6", KeyCode.L, 1);
        if (Input.GetKeyDown(KeyCode.Q))
            Managers.AchievementSystem.ReceiveReport("TUTO7", KeyCode.Q, 1);
        if (Input.GetKeyDown(KeyCode.E))
            Managers.AchievementSystem.ReceiveReport("TUTO7", KeyCode.E, 1);
    }

    public void Clear()
    {
        OnTutorialClear = null;
        OnScriptPhaseUpdate = null;
        OnDisableLinkedWall = null;
        OnEnableDontGoBackWall = null;
        OnEnableDummy = null;
        OnShootDummy = null;
    }
}
