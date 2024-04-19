using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_MissileLauncher : WeaponBase
{
    private Animator _anim;

    private int _usingHash;
    private int _upHash;
    private readonly string USING_PARAMETER = "IsUsing";
    private readonly string UP_NAME = "Launcher_Up";

    public override void Setup(int partID, Define.Parts_Location type, LayerMask layerMask)
    {
        base.Setup(partID, type, layerMask);

        _anim = GetComponentInChildren<Animator>();
        _usingHash = Animator.StringToHash(USING_PARAMETER);
        _upHash = Animator.StringToHash(UP_NAME);
    }

    public override void UseWeapon(Transform[] muzzlePoints)
    {
        if (IsCoolDown || Ammo <= 0)
            return;
        
        StartCoroutine(Co_UseWeapon(muzzlePoints));
    }

    public IEnumerator Co_UseWeapon(Transform[] muzzlePoints)
    {
        IsCoolDown = true;
        _anim.Play(_upHash);
        _anim.SetBool(_usingHash, true);
        float animLength = Util.GetCurrentAnimationClipLength(_anim);        
        yield return Util.GetWaitSeconds(animLength + 0.5f);

        foreach (Transform muzzle in muzzlePoints)
        {
            for (int i = 0; i < _partData.ProjectilesPerShot; i++)
            {
                if (Ammo == 0)
                    continue;
                Ammo--;
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_LaunchRocket, Vector3.zero);

                Vector3 freeFireTarget = GetFreeFireDest();

                GameObject bullet = CreateBullet(muzzle);

                PlayerProjectile missile = bullet.GetComponent<PlayerProjectile>();
                missile.Setup(BulletSpeed, Damage, _partData.IsSplash, freeFireTarget, _target, _partData.ExplosiveRange);

                yield return Util.GetWaitSeconds(FireRate);
            }
        }
        
        _anim.SetBool(_usingHash, false);
        yield return Util.GetWaitSeconds(CoolDownTime);
        IsCoolDown = false;
    }
}
