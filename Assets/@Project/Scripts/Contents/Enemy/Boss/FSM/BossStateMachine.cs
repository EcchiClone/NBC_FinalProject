using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateMachine
{
    public Boss Boss { get; private set; }
    public BossStateProvider Provider { get; protected set; }
    public BossBaseState CurrentState { get; set; }

    public BossStateMachine(Boss boss)
    {
        Boss = boss;
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
