using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : Weapon_Primary
{
    public override void UseWeapon_Primary(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            GameObject bullet = Instantiate(WeaponSO.bulletPrefab);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
        }        
    }
}
