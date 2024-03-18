using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    [SerializeField] GameObject[] _boosterEffects;    

    protected Module _module;
    protected PlayerStateMachine _stateMachine;

    public virtual void Setup(Module module, PlayerStateMachine stateMachine) 
    {
        _module = module;
        _stateMachine = stateMachine;
    }

    public virtual void BoostOnOff(bool isActive)
    {
        foreach(var effect in _boosterEffects)
            effect.SetActive(isActive);
    }    
}
