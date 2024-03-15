using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] string walkParameterName = "Walk";
    [SerializeField] string walkForNAftParameterName = "ForAndAft";
    [SerializeField] string walkLeftNRightParameterName = "LeftAndRight";
    [SerializeField] string jumpParameterName = "Jump";
    [SerializeField] string dashParameterName = "Dash";
    [SerializeField] string nonCombatParameterName = "@NonCombat";
    [SerializeField] string combatParameterName = "@Combat";    

    public int WalkParameterHash { get; private set; }
    public int WalkFnAParameterHash { get; private set; }
    public int WalkLnRParameterHash { get; private set; }
    public int JumpParameterName { get; private set; }
    public int NonCombatParameterName { get; private set; }
    public int CombatParameterName { get; private set; }
    public int DashParameterName { get; private set; }

    public void Init()
    {
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        WalkFnAParameterHash = Animator.StringToHash(walkForNAftParameterName);
        WalkLnRParameterHash = Animator.StringToHash(walkLeftNRightParameterName);
        JumpParameterName = Animator.StringToHash(jumpParameterName);
        NonCombatParameterName = Animator.StringToHash(nonCombatParameterName);
        CombatParameterName = Animator.StringToHash(combatParameterName);
        DashParameterName = Animator.StringToHash(dashParameterName);
    }
}
