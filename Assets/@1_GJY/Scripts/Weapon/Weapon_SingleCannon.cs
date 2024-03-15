using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : Weapon_Primary
{
    public override void UseWeapon_Primary(Transform[] muzzlePoints)
    {
        RandomDirectionShot(muzzlePoints);
    }
}
