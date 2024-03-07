using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] string walkParameterName = "IsWalk";

    public int WalkParameterHash { get; private set; }

    public void Init()
    {
        WalkParameterHash = Animator.StringToHash(walkParameterName);
    }
}
