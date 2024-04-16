using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RocketLauncher : WeaponBase
{
    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (IsCoolDown || Ammo <= 0)
            return;

        StartCoroutine(Co_UseWeapon(muzzlePoints));
    }

    public IEnumerator Co_UseWeapon(Transform[] muzzlePoints)
    {
        IsCoolDown = true;

        foreach (Transform muzzle in muzzlePoints)
        {
            for (int i = 0; i < _partData.ProjectilesPerShot; i++)
            {
                if (Ammo == 0)
                    continue;
                Ammo--;

                Vector3 freeFireTarget = GetFreeFireDest();
                GameObject bullet = CreateBullet(muzzle);                

                PlayerProjectile missile = bullet.GetComponent<PlayerProjectile>();
                missile.Setup(_partData.BulletSpeed, _partData.Damage, _partData.IsSplash, freeFireTarget, _target);
                
                yield return Util.GetWaitSeconds(_partData.FireRate);
            }
        }

        yield return Util.GetWaitSeconds(_partData.CoolDownTime);
        IsCoolDown = false;
    }
}
