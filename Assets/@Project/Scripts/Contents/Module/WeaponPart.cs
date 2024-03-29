using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : BasePart
{
    [SerializeField] Transform _muzzlePoint;

    protected IWeapon _weapon;

    public override void Setup(Module module)
    {
        base.Setup(module);
    }
}
