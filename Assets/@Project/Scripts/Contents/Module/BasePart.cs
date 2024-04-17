using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    [field:SerializeField] public int ID { get; private set; }

    [SerializeField] GameObject[] _boosterEffects;

    protected Module Module { get; private set; }
    protected Define.Parts_Location _type;    

    public virtual void Setup(Define.Parts_Location type, Module module)
    {
        Module = module;
        _type = type;        
    }

    public virtual void BoostOnOff(bool isActive)
    {
        foreach (var effect in _boosterEffects)
            effect.SetActive(isActive);
    }
}
