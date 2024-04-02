using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperPart : BasePart
{
    public enum WeaponType
    {
        LeftArm,
        RightArm,
        LeftShoulder,
        RightShoulder,
    }

    [field: SerializeField] public Transform WeaponTilt { get; private set; }
    [field: SerializeField] public Transform[] WeaponPositions { get; private set; }    

    public override void Setup(Module module)
    {
        base.Setup(module);
    }
}
