using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_MissileLauncher : WeaponBase
{
    private Animator _anim;

    private int _usingHash;
    private readonly string USING_PARAMETER = "IsUsing";

    public override void Setup(int partID, Define.PartsType type, LayerMask layerMask)
    {
        base.Setup(partID, type, layerMask);

        _anim = GetComponentInChildren<Animator>();
        _usingHash = Animator.StringToHash(USING_PARAMETER);
    }

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (_isCoolDown || Ammo <= 0)
            return;
        
        StartCoroutine(Co_UseWeapon(muzzlePoints));
    }

    public IEnumerator Co_UseWeapon(Transform[] muzzlePoints)
    {
        _isCoolDown = true;
        _anim.SetBool(_usingHash, true);
        float animLength = Util.GetCurrentAnimationClipLength(_anim);
        Debug.Log(animLength);
        yield return Util.GetWaitSeconds(animLength);

        foreach (Transform muzzle in muzzlePoints)
        {
            for (int i = 0; i < _partData.ProjectilesPerShot; i++)
            {
                Ammo--;
                Vector3 freeFireTarget = GetFreeFireDest();

                GameObject bullet = CreateBullet(muzzle);

                PlayerProjectile missile = bullet.GetComponent<PlayerProjectile>();
                missile.Setup(_partData.BulletSpeed, _partData.Damage, freeFireTarget, _target);

                yield return Util.GetWaitSeconds(_partData.FireRate);
            }
        }

        _anim.SetBool(_usingHash, false);
        yield return Util.GetWaitSeconds(_partData.CoolDownTime);
        _isCoolDown = false;        
    }
}
