using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : WeaponBase
{
    private float _delayTime = float.MaxValue;

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if(_delayTime < _partData.FireRate)
        {
            _delayTime += Time.deltaTime;
            return;
        }

        _delayTime = 0;
        ShotCannon(muzzlePoints);        
    }

    private void ShotCannon(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            GameObject bullet = CreateBullet(muzzle);

            Quaternion rotation = Util.RandomDirectionFromMuzzle(_partData.ShotErrorRange);
            bullet.transform.rotation *= rotation;

            PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
            projectile.Setup(_partData.BulletSpeed, Vector3.zero, _target);
        }
    }
}
