using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Arm : WeaponBase, IWeapon
{
    public override void Setup(Transform bodyTransform, LayerMask layerMask)
    {
        base.Setup(bodyTransform, layerMask);
    }

    public override void UseWeapon(Transform[] muzzlePoints) { }
}
