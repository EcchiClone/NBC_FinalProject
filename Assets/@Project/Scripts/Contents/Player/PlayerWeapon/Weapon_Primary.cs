using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Primary : WeaponBase, IWeapon
{
    public override void Setup()
    {
        base.Setup();
    }

    public virtual void UseWeapon_Primary(Transform[] muzzlePoints) { }    

    public virtual IEnumerator UseWeapon_Secondary(Transform[] muzzlePoints) { yield return null; } // Not Used
}
