using System;
using System.Collections.Generic;

public abstract class BaseStateProvider
{
    protected BaseStateMachine _context;

    private Dictionary<Enum, BaseState> _states = new Dictionary<Enum, BaseState>();

    public BaseStateProvider(BaseStateMachine context)
    {
        _context = context;
    }

    public BaseState GetState<StateEnum>(StateEnum stateEnum) where StateEnum : Enum
    {
        if (_states.ContainsKey(stateEnum))
        {
            return _states[stateEnum];
        }
        else
        {
            throw new InvalidOperationException("잘못된 상태 요구");
        }
    }

    protected virtual void SetState<StateEnum>(StateEnum stateEnum, BaseState state) where StateEnum : Enum
    {
        if (_states.ContainsKey(stateEnum))
            return;

        _states[stateEnum] = state;
    }
}
