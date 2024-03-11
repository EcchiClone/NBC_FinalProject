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
            foreach(Transform muzzle in muzzlePoints)
            {
                GameObject bullet = Instantiate(WeaponSO.bulletPrefab);
                bullet.transform.position = muzzle.position;
                bullet.transform.rotation = muzzle.rotation;
            }
            yield return _coolDown;
        }        

        StartCoroutine(CoWaitCoolDownTime());
    }    
}
