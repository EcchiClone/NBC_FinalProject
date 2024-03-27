using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPart : BasePart
{
    [SerializeField] GameObject[] _footSparks;

    public override void Setup(Module module)
    {
        base.Setup(module);
    }

    public override void BoostOnOff(bool isActive)
    {
        base.BoostOnOff(isActive);

        foreach (var spark in _footSparks)
            spark.SetActive(isActive);
    }
}