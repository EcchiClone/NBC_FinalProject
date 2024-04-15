using System;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class TutorialManager
{
    public event Action OnTutorialStart;
    public event Action OnTutorialClear;
    public event Action OnScriptPhaseUpdate;

    public Achievement CurrentMission { get; private set; }
    public bool IsMissioning { get; private set; }

    private UI_Popup _skipPopup;

    private readonly float BLIND_TIME = 2f;

    public void TutorialStart()
    {
        Managers.Input.OnInputKeyDown += TutorialKeyInput;

        UI_BlinderPopup blinder = Managers.UI.ShowPopupUI<UI_BlinderPopup>();
        blinder.DoOpenBlind(BLIND_TIME, () => Managers.UI.ShowPopupUI<UI_TutorialDialoguePopup>());

        // 락을 걸어야 됨
        OnTutorialStart?.Invoke();
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
    }

    public void Clear()
    {
        OnTutorialStart = null;
        OnTutorialClear = null;
    }
}
