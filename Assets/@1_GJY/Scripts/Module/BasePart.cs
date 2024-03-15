using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{    
    [field: SerializeField] public int ID { get; private set; }

    protected Module _module;
    protected PlayerStateMachine _stateMachine;

    public virtual void Setup(Module module, PlayerStateMachine stateMachine) 
    {
        _module = module;
        _stateMachine = stateMachine;
    }
}
