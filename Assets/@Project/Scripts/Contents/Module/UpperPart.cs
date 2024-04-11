using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperPart : BasePart
{
    [field: SerializeField] public Transform[] WeaponPositions { get; private set; }    

    public override void Setup(Define.Parts_Location type, Module module)
    {
        base.Setup(type, module);
    }
}
