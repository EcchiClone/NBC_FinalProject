using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_HE_TACannon : WeaponBase
{
    [SerializeField] Transform _pivotVertical;
    [SerializeField] Transform _pivotHead;

    private Animator _anim;

    private Quaternion _SteadyPos;
    private int _usingHash;
    private int _upHash;
    private readonly string USING_PARAMETER = "IsUsing";
    private readonly string UP_NAME = "Launcher_Up";

    public override void Setup(int partID, Define.Parts_Location type, LayerMask layerMask)
    {
        base.Setup(partID, type, layerMask);

        _anim = GetComponent<Animator>();
        _usingHash = Animator.StringToHash(USING_PARAMETER);
        _upHash = Animator.StringToHash(UP_NAME);
    }

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (IsCoolDown || Ammo <= 0)
            return;

        StartCoroutine(Co_UseWeapon(muzzlePoints));
    }

    private IEnumerator Co_UseWeapon(Transform[] muzzlePoints)
    {
        IsCoolDown = true;

        _anim.Play(_upHash);
        _anim.SetBool(_usingHash, true);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_AirPressure, Vector3.zero);
        float animLength = Util.GetCurrentAnimationClipLength(_anim);        

        yield return Util.GetWaitSeconds(animLength + 0.5f);        
        yield return StartCoroutine(Co_Targeting());

        foreach (Transform muzzle in muzzlePoints)
        {
            for (int i = 0; i < _partData.ProjectilesPerShot; i++)
            {
                if (Ammo == 0)
                    continue;
                Ammo--;

                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_LaserCannon_Shot, Vector3.zero);
                Vector3 freeFireTarget = GetFreeFireDest();
                if (_target != null)
                    transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
                GameObject bullet;
                if (IsHitTarget(muzzle, out Transform target))
                    bullet = CreateBullet(target);
                else
                    bullet = CreateBullet(muzzle);

                PlayerProjectile projectileCannon = bullet.GetComponent<PlayerProjectile>();
                projectileCannon.Setup(BulletSpeed, Damage, _partData.IsSplash, freeFireTarget, _target, _partData.ExplosionRange);

                yield return Util.GetWaitSeconds(FireRate);
            }
        }

        yield return StartCoroutine(Co_Release());

        _anim.SetBool(_usingHash, false);
        yield return Util.GetWaitSeconds(CoolDownTime);
        IsCoolDown = false;
    }

    private IEnumerator Co_Targeting()
    {
        if (_target == null)
            yield break;

        Vector3 weaponToLookAt = _target.position - transform.position;
        
        Quaternion currentHeadRot = transform.rotation;
        Quaternion targetHeadRotation = Quaternion.LookRotation(weaponToLookAt);
        _SteadyPos = transform.localRotation;

        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;

            transform.rotation = Quaternion.Slerp(currentHeadRot, targetHeadRotation, percent);
            yield return null;
        }

        transform.rotation = targetHeadRotation;
    }

    private IEnumerator Co_Release()
    {
        Util.GetWaitSeconds(0.5f);

        Quaternion currentHeadRot = transform.localRotation;
        Quaternion targetHeadRotation = _SteadyPos;

        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;

            transform.localRotation = Quaternion.Slerp(currentHeadRot, targetHeadRotation, percent);
            yield return null;
        }
    }

    private Transform IsHitTarget(Transform muzzle, out Transform hitTarget)
    {        
        if(Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f))
        {
            if (hit.transform.TryGetComponent(out ITarget target) == true)
                return hitTarget = target.Transform;             
        }
        return hitTarget = null;
    }
}
