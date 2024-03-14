using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{    
    [field: SerializeField] public int ID { get; private set; }

    protected Module _module;

    public virtual void Setup(Module module) 
    {
        _module = module;
    }
}
