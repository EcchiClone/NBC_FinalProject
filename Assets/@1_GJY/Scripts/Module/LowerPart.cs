using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPart : BasePart
{
    public LowerPartsSO lowerSO;

    [SerializeField] GameObject[] _footSparks;

    public override void Setup(Module module, PlayerStateMachine stateMachine)
    {
        base.Setup(module, stateMachine);
    }

    public override void BoostOnOff(bool isActive)
    {
        base.BoostOnOff(isActive);

        foreach (var spark in _footSparks)
            spark.SetActive(isActive);
    }
}
