using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponSO {  get; private set; }

    public virtual void Setup() { }
}
