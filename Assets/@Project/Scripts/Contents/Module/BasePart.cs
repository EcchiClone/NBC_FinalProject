using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    public int ID { get; private set; }

    [SerializeField] GameObject[] _boosterEffects;

    protected Module Module { get; private set; }

    public void SetID(int id) => ID = id;

    public virtual void Setup(Module module) => Module = module;

    public virtual void BoostOnOff(bool isActive)
    {
        foreach (var effect in _boosterEffects)
            effect.SetActive(isActive);
    }
}
