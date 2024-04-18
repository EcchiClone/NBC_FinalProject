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
    private readonly float LASER_DAMAGING_TIME = 0.02f;

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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Laser_Charge, Vector3.zero);
        yield return Util.GetWaitSeconds(_partData.FireRate);

        StartCoroutine(GunFire(muzzlePoints));
    }

    private IEnumerator GunFire(Transform[] muzzlePoints)
    {
        _chargingEffect.SetActive(false);
        Transform muzzle = muzzlePoints[0];
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Laser_Shot, Vector3.zero);

        float current = 0;
        float percent = 0;

        float startLaserWidth = _laser.startWidth;

        while(percent < 1)
        {
            current += LASER_DAMAGING_TIME;
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
                if (hit.transform.TryGetComponent(out ITarget target) == true)
                    target.GetDamaged(Damage);

            yield return Util.GetWaitSeconds(LASER_DAMAGING_TIME);
        }

        Ammo--;
        LaserClear(startLaserWidth, muzzle);

        yield return Util.GetWaitSeconds(CoolDownTime);
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
