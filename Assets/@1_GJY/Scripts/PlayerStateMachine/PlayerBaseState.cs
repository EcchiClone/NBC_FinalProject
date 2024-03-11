using UnityEngine;

public abstract class PlayerBaseState
{
    protected bool IsRootState { get; set; } = false;

    protected PlayerStateMachine Context { get; private set; }
    protected PlayerStateFactory Factory { get; private set; }

    protected PlayerBaseState _currentSuperState;
    protected PlayerBaseState _currentSubState;

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        Context = context;
        Factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitailizeSubState();
    public void UpdateStates() 
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateState();
    }

    protected void SwitchState(PlayerBaseState newState) // 상태 전환
    {
        // 현재 상태 종료 로직 수행
        ExitState();

        // 다음 상태 시작
        newState.EnterState();

        if (IsRootState)
            Context.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);

        Debug.Log($"현재 State : {Context.CurrentState}");
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        _currentSubState.SetSuperState(this);
    }

    protected void StartAnimation(int animationHash)
    {
        Context.Anim.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        Context.Anim.SetBool(animationHash, false);
    }
}
