using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : BasePart
{
    [SerializeField] Transform[] _muzzlePoints;
    [SerializeField] LayerMask _groundLayer;    

    public WeaponBase Weapon { get; private set; }

    public override void Setup(Define.PartsType type, Module module)
    {
        base.Setup(type, module);
        Weapon = GetComponent<WeaponBase>();
        Weapon.Setup(ID, type, _groundLayer);
    }

    public void UseWeapon() => Weapon.UseWeapon(_muzzlePoints);
}
