using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RocketLauncher : WeaponBase
{
    private bool _isCoolDown = false;

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (_isCoolDown)
            return;

        StartCoroutine(Co_UseWeapon(muzzlePoints));
    }

    public IEnumerator Co_UseWeapon(Transform[] muzzlePoints)
    {
        _isCoolDown = true;

        foreach (Transform muzzle in muzzlePoints)
        {
            for (int i = 0; i < _partData.ProjectilesPerShot; i++)
            {
                Vector3 freeFireTarget = GetFreeFireDest();

                GameObject bullet = CreateBullet(muzzle);

                PlayerProjectile missile = bullet.GetComponent<PlayerProjectile>();
                missile.Setup(_partData.BulletSpeed, freeFireTarget, _target);

                yield return Util.GetWaitSeconds(_partData.FireRate);
            }
        }        

        yield return Util.GetWaitSeconds(_partData.CoolDownTime);
        _isCoolDown = false;
    }
}
