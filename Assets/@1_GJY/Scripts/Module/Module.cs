using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [field: SerializeField] public Transform LowerPosition { get; private set; }

    private Transform _upperPivot;
    private Transform _weaponPivot;

    public void SetPivot(Transform pivot)
    {
        if (_upperPivot == null)
        {
            _upperPivot = pivot;
            return;
        }
        _weaponPivot = pivot;
    }
}
