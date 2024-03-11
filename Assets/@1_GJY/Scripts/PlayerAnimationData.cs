using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] string walkParameterName = "IsWalk";
    [SerializeField] string walkForNAftParameterName = "ForAndAft";
    [SerializeField] string walkLeftNRightParameterName = "LeftAndRight";

    public int WalkParameterHash { get; private set; }
    public int WalkFnAParameterHash { get; private set; }
    public int WalkLnRParameterHash { get; private set; }

    public void Init()
    {
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        WalkFnAParameterHash = Animator.StringToHash(walkForNAftParameterName);
        WalkLnRParameterHash = Animator.StringToHash(walkLeftNRightParameterName);
    }
}
