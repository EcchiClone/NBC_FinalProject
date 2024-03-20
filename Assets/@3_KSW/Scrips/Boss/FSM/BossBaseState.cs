using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    protected bool IsRootState { get; set; } = false;
    protected BossStateMachine Context { get; private set; }
    protected BossStateProvider Provider { get; private set; }
    protected BossBaseState _currentSuperState;
    protected BossBaseState _currentSubState;

    public BossBaseState(BossStateMachine context, BossStateProvider provider)
    {
        Context = context;
        Provider = provider;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates(); // 상태 나타내는 bool변수 두고 어떤 상태로 바꿀지 결정
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();
    }

    protected void SwitchState(BossBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (IsRootState) // 루트라면(Grounded나 Fall이라면) 루트끼리 바뀌게
        {
            Context.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    public void SetSuperState(BossBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    public void SetSubState(BossBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
