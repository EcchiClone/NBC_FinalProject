using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseState<StateEnum> where StateEnum : Enum
{
    protected bool IsRootState { get; set; } = false;
    protected BaseStateMachine Context { get; private set; }
    protected BaseStateProvider<StateEnum> Provider { get; private set; }
    protected BaseState<StateEnum> _currentSuperState;
    protected BaseState<StateEnum> _currentSubState;

    public BaseState(BaseStateMachine context, BaseStateProvider<StateEnum> provider)
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

    protected void SwitchState(BaseState<StateEnum> newState)
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

    public void SetSuperState(BaseState<StateEnum> newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    public void SetSubState(BaseState<StateEnum> newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
