using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseStateMachine
{
    public Entity Entity { get; private set; }
    public BaseStateProvider Provider { get; protected set; }
    public BaseState CurrentState { get; set; }
    
    public Enemy_Sound Sound { get; private set; }

    public BaseStateMachine(Entity entity)
    {
        Entity = entity;
        Initialize();
        Sound = Entity.GetComponent<Enemy_Sound>();
    }

    public abstract void Initialize();

    public abstract void Reset();

    public void Update()
    {
        
            CurrentState?.UpdateStates();
    }
}
