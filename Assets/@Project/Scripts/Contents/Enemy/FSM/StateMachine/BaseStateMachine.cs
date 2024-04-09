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
    public Animator Anim { get; private set; }

    public BaseStateMachine(Entity entity)
    {
        Entity = entity;
        Initialize();
        Anim = Entity.GetComponent<Animator>();
    }

    public abstract void Initialize();

    public void Update()
    {
        
            CurrentState?.UpdateStates();
    }
}
