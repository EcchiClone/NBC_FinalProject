using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : BasePart
{
    [SerializeField] Transform[] _muzzlePoints;
    [SerializeField] LayerMask _groundLayer;

    protected WeaponBase _weapon;

    public override void Setup(Module module)
    {
        base.Setup(module);
        _weapon = GetComponent<WeaponBase>();
        _weapon.Setup(ID, transform, _groundLayer);
    }

    public void UseWeapon() => _weapon.UseWeapon(_muzzlePoints);
}
