using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperPart : BasePart
{
    [field: SerializeField] public Transform WeaponTilt { get; private set; }
    [SerializeField] Transform[] _primaryMuzzles;
    [SerializeField] Transform[] _secondaryMuzzles;

    private Weapon_Primary _primary;
    private Weapon_Secondary _secondary;

    public Weapon_Primary Primary 
    { get 
        { if (_primary == null) 
                return GetComponent<Weapon_Primary>(); 
            return _primary; 
        } 
    }
    public Weapon_Secondary Secondary 
    { get 
        { if (_secondary == null) 
                return GetComponent<Weapon_Secondary>(); 
            return _secondary; 
        } 
    }

    private float _primaryFireRate = float.MaxValue;
    private float _secondaryCoolDown = float.MaxValue;

    public override void Setup(Module module)
    {
        base.Setup(module);

        if (!Module.IsPlayable)
            return;

        Primary.Setup();
        Secondary.Setup();
    }

    private void Update()
    {
        if (!Module.IsPlayable)
            return;

        if (_primaryFireRate < Primary.WeaponSO.fireRate)
            _primaryFireRate += Time.deltaTime;
        if (_secondaryCoolDown < Secondary.WeaponSO.coolDownTime)
            _secondaryCoolDown += Time.deltaTime;
    }

    public void UseWeapon_Primary()
    {
        if (_primaryFireRate < Primary.WeaponSO.fireRate)
            return;

        _primaryFireRate = 0;
        Primary.UseWeapon_Primary(_primaryMuzzles);
    }

    public void UseWeapon_Secondary()
    {
        if (_secondaryCoolDown < Secondary.WeaponSO.coolDownTime)
            return;

        _secondaryCoolDown = 0;
        StartCoroutine(Secondary.UseWeapon_Secondary(_secondaryMuzzles));
    }
}
