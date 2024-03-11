using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePart : MonoBehaviour
{
    // LowerPart == UpperPoint / UpperPart == WeaponPoint
    [field: SerializeField] public Transform PivotPoint { get; private set; }

    public void Setup()
    {

    }
}
