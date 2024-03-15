using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RocketLauncher : Weapon_Secondary
{    
    public override IEnumerator UseWeapon_Secondary(Transform[] muzzlePoints)
    {
        if (_isCoolDown)
        {
            Debug.Log("CoolDownTime");
            yield break;
        }

        base.UseWeapon_Secondary(muzzlePoints);        

        for (int i = 0; i < WeaponSO.projectilesPerShot; i++)
        {
            RandomDirectionShot(muzzlePoints);

            yield return _fireRate;
        }        

        StartCoroutine(CoWaitCoolDownTime());
    }    
}
