using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : Weapon_Arm
{
    public override void UseWeapon(Transform[] muzzlePoints)
    {
        ShotBullets(muzzlePoints);
    }

    protected void ShotBullets(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            Vector3 freeFirePoint = Vector3.up * 100f;
            if (_target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(_weaponTransform.position, Camera.main.transform.forward, out hit, float.MaxValue, _groundLayer))
                    freeFirePoint = hit.point;
            }

            GameObject go = EnemyBulletPoolManager.instance.GetGo(WeaponSO.bulletName);
            go.transform.position = muzzle.position;
            go.transform.rotation = muzzle.rotation;

            Quaternion rotation = Util.RandomDirectionFromMuzzle(WeaponSO.shotErrorRange);
            go.transform.rotation *= rotation;

            PlayerProjectile projectile = go.GetComponent<PlayerProjectile>();
            projectile.Setup(WeaponSO.speed, freeFirePoint, _target);
        }
    }
}
