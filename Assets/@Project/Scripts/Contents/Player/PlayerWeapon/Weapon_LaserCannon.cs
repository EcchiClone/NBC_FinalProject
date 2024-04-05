using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_LaserCannon : WeaponBase
{
    [SerializeField] GameObject _chargingEffect;
    [SerializeField] LineRenderer _laser;
    [SerializeField] AnimationCurve _laserCurve;
    [SerializeField] LayerMask _enemyLayer;

    private readonly float LASER_CUT_TIME = 1f;

    public override void UseWeapon(Transform[] muzzlePoints)
    {        
        if (IsCoolDown)
            return;

        IsCoolDown = true;        
        StartCoroutine(Co_ChargingLaser(muzzlePoints));
    }

    private IEnumerator Co_ChargingLaser(Transform[] muzzlePoints)
    {        
        _chargingEffect.SetActive(true);
        yield return Util.GetWaitSeconds(_partData.FireRate);

        StartCoroutine(GunFire(muzzlePoints));
    }

    private IEnumerator GunFire(Transform[] muzzlePoints)
    {
        _chargingEffect.SetActive(false);
        Transform muzzle = muzzlePoints[0];

        float current = 0;
        float percent = 0;

        float startLaserWidth = _laser.startWidth;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / LASER_CUT_TIME;

            float maxLength = 100f;
            Ray ray = new Ray(muzzle.position, muzzle.forward);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, maxLength, _enemyLayer);

            Vector3 hitPoint = isHit ? hit.point : muzzle.position + muzzle.forward * maxLength;

            _laser.SetPosition(0, muzzle.position);
            _laser.SetPosition(1, hitPoint);
            _laser.enabled = true;

            _laser.startWidth = Mathf.Lerp(startLaserWidth, 0, _laserCurve.Evaluate(percent));
            _laser.endWidth = Mathf.Lerp(startLaserWidth, 0, _laserCurve.Evaluate(percent));

            if (isHit)
                if (hit.transform.TryGetComponent(out Entity boss) == true)
                    boss.GetDamaged(_partData.Damage);

            yield return null;
        }

        Ammo--;
        LaserClear(startLaserWidth, muzzle);

        yield return Util.GetWaitSeconds(_partData.CoolDownTime);
        IsCoolDown = false;
    }

    private void LaserClear(float startLaserWidth, Transform muzzle)
    {
        _laser.enabled = false;
        _laser.SetPosition(0, muzzle.position);
        _laser.SetPosition(1, muzzle.position);
        _laser.startWidth = startLaserWidth;
        _laser.endWidth = startLaserWidth;
    }
}
