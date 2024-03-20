using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Secondary : WeaponBase, IWeapon
{
    protected WaitForSeconds _fireRate;
    protected WaitForSeconds _coolDown;

    protected bool _isCoolDown = false;

    public override void Setup()
    {
        base.Setup();

        _fireRate = new WaitForSeconds(WeaponSO.fireRate);
        _coolDown = new WaitForSeconds(WeaponSO.coolDownTime);
    }    

    public virtual void UseWeapon_Primary(Transform[] muzzlePoints) { } // Not used    

    public virtual IEnumerator UseWeapon_Secondary(Transform[] muzzlePoints) { _isCoolDown = true; yield return null; } 
    
    protected IEnumerator CoWaitCoolDownTime()
    {
        yield return _coolDown;

        _isCoolDown = false;
    }
}
