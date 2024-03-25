using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void UseWeapon_Primary(Transform[] muzzlePoints);
    IEnumerator UseWeapon_Secondary(Transform[] muzzlePoints);    
}
