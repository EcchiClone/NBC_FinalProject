using UnityEngine;

public class Weapon_SingleCannon : WeaponBase
{
    private float _delayTime = float.MaxValue;

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (IsCoolDown)
            return;

        if (_delayTime >= FireRate && Ammo > 0)
        {
            _delayTime = 0;
            Ammo--;
            GunFire(muzzlePoints);
        }
    }

    private void Update()
    {
        if (_delayTime < FireRate)
            _delayTime += Time.deltaTime;
    }

    private void GunFire(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            CreateMuzzleEffect(muzzle).Setup();
            GameObject bullet = CreateBullet(muzzle);

            Quaternion rotation = Util.RandomDirectionFromMuzzle(_partData.ShotErrorRange);
            bullet.transform.rotation *= rotation;
            // TODO: 발사음 추가

            PlayerProjectile projectile = bullet.GetComponent<PlayerProjectile>();
            projectile.Setup(BulletSpeed, Damage, _partData.IsSplash, Vector3.zero, _target);
        }
    }
}
