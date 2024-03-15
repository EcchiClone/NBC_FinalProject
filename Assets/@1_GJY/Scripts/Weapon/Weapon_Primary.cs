using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Primary : WeaponBase, IWeapon
{
    public override void Setup(PlayerStateMachine stateMachine)
    {
        base.Setup(stateMachine);
    }

    public virtual void UseWeapon_Primary(Transform[] muzzlePoints) { }    

    public virtual IEnumerator UseWeapon_Secondary(Transform[] muzzlePoints) { yield return null; } // Not Used
}
