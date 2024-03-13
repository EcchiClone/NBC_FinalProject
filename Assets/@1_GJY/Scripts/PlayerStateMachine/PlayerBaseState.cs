using UnityEngine;

public enum RootStateType
{
    NonCombat,
    Combat,
}

public abstract class PlayerBaseState : IPlayerState
{
    public RootStateType StateType { get; protected set; }

    protected bool IsRootState { get; set; } = false;

    protected PlayerStateMachine Context { get; private set; }
    protected PlayerStateFactory Factory { get; private set; }

    public PlayerBaseState _currentSuperState; // 임시 public
    public PlayerBaseState _currentSubState; // 임시 public

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        Context = context;
        Factory = factory;
    }
    
    public virtual void EnterState() { } // State Switch 시 실행 (또는 StateMachine Awake에서 첫 실행)
    public virtual void UpdateState() { } // RootState(Non-Combat / Combat)포함 하위에 SubState가 있다면 항시 Update
    public virtual void ExitState() { } // State Switch 시 실행
    public virtual void CheckSwitchStates() { } // Update문에서 SwitchState 조건을 확인 (Combat -> Non-Combat / Grounded -> Jump 조건등)
    public virtual void InitailizeSubState() { } // EnterState 실행 시 현재 State의 SetSub해야 할 State가 있는지 확인

    public void UpdateStates() // StateMachine의 Update문에서 실행시켜줄 메서드. Update
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
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
