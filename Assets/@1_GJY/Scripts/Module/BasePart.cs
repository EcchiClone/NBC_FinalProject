using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    [SerializeField] GameObject[] _boosterEffects;    

    protected Module _module;    

    public virtual void Setup(Module module) 
    {
        _module = module;        
    }

    public virtual void BoostOnOff(bool isActive)
    {
        foreach(var effect in _boosterEffects)
            effect.SetActive(isActive);
    }    
}
