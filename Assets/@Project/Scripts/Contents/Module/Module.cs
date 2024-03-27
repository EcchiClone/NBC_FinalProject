using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [field: SerializeField] public Transform LowerPivot { get; private set; }

    public Transform UpperPivot { get; private set; }
    public Transform WeaponPivot { get; private set; }  

    public LowerPart CurrentLower { get; private set; }
    public UpperPart CurrentUpper { get; private set; }
    
    public bool IsPlayable { get; private set; }

    private void Awake()
    {
        if (GetComponent<PlayerStateMachine>() != null)
        {
            IsPlayable = true;
            return;
        }
    }

    public void Setup(LowerPart lowerPart, UpperPart upperPart)
    {
        CurrentLower = lowerPart;
        CurrentUpper = upperPart;
    }

    public void SetPivot(Transform pivot)
    {
        if (UpperPivot == null)
        {
            UpperPivot = pivot;
            return;
        }
        WeaponPivot = pivot;
    }
}
