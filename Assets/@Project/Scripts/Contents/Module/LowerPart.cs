using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPart : BasePart
{
    [field: SerializeField] public Transform UpperPositions { get; private set; }

    [SerializeField] GameObject[] _footSparks;

    public override void Setup(Define.PartsType type, Module module)
    {
        base.Setup(type, module);
    }

    public override void BoostOnOff(bool isActive, bool isGround)
    {
        base.BoostOnOff(isActive, isGround);

        if (isGround)
            foreach (var spark in _footSparks)
                spark.SetActive(isActive);
    }
}
