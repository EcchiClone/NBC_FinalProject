using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : IWeapon
{
    [field: SerializeField] public WeaponSO WeaponSO { get; private set; }    

    protected Transform _target;
    protected Transform _weaponTransform;
    protected LayerMask _groundLayer;

    public virtual void UseWeapon(Transform[] muzzlePoints) { }    

    public virtual void Setup(Transform bodyTransform, LayerMask layerMask) 
    {
        Managers.ActionManager.OnLockOnTarget += Targeting;
        Managers.ActionManager.OnReleaseTarget += Release;

        _weaponTransform = bodyTransform;
        _groundLayer = layerMask;
    }

    private void Targeting(Transform target) => _target = target;
    private void Release() => _target = null;
}
