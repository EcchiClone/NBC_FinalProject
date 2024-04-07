using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    public int ID { get; private set; }

    [SerializeField] GameObject[] _boosterEffects;

    protected Module Module { get; private set; }
    protected Define.PartsType _type;

    public void SetID(int id) => ID = id;

    public virtual void Setup(Define.PartsType type, Module module)
    {
        Module = module;
        _type = type;
    }

    public virtual void BoostOnOff(bool isActive)
    {
        foreach (var effect in _boosterEffects)
            effect.SetActive(isActive);
    }

    public virtual void BoostOnOff(bool isActive, bool isGround)
    {
        BoostOnOff(isActive);
    }
}
