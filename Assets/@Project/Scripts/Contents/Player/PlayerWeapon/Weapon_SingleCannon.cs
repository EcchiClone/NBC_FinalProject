using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : WeaponBase
{
    private float _delayTime = float.MaxValue;

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (_isCoolDown)
            return;

        if (_delayTime >= _partData.FireRate)
        {
            _delayTime = 0;
            GunFire(muzzlePoints);
        }
    }

    private void Update()
    {
        if (_delayTime < _partData.FireRate)
            _delayTime += Time.deltaTime;
    }

    private void GunFire(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            GameObject bullet = CreateBullet(muzzle);

            Quaternion rotation = Util.RandomDirectionFromMuzzle(_partData.ShotErrorRange);
            bullet.transform.rotation *= rotation;

            PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
            projectile.Setup(_partData.BulletSpeed, _partData.Damage, Vector3.zero, _target);
        }
    }
}
