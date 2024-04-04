using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine
{
    public Entity Entity { get; private set; }
    public BaseStateProvider Provider { get; protected set; }
    public BaseState CurrentState { get; set; }

    public BaseStateMachine(Entity boss)
    {
        Entity = boss;
        Initialize();
    }

    public abstract void Initialize();

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateStates();
        }
    }
}
