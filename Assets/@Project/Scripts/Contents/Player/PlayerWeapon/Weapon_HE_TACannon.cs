using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_HE_TACannon : WeaponBase
{
    [SerializeField] Transform _pivotVertical;
    [SerializeField] Transform _pivotHead;

    private Animator _anim;    

    private int _usingHash;
    private int _upHash;
    private readonly string USING_PARAMETER = "IsUsing";
    private readonly string UP_NAME = "Launcher_Up";

    public override void Setup(int partID, Define.PartsType type, LayerMask layerMask)
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
                Vector3 freeFireTarget = GetFreeFireDest();                        
                GameObject bullet = CreateBullet(muzzle);

                PlayerProjectile cannon = bullet.GetComponent<PlayerProjectile>();
                cannon.Setup(_partData.BulletSpeed, _partData.Damage, _partData.IsSplash, freeFireTarget, _target);

                yield return Util.GetWaitSeconds(_partData.FireRate);
            }
        }        

        _anim.SetBool(_usingHash, false);
        yield return Util.GetWaitSeconds(_partData.CoolDownTime);
        IsCoolDown = false;
    }    
}
