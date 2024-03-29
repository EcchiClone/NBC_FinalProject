using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [field: SerializeField] public Transform LowerPosition { get; private set; }    

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
}
