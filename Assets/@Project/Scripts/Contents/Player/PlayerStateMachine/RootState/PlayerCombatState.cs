using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    private float _timeToNonCombat;    

    public PlayerCombatState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        StateType = RootStateType.Combat;
        StartAnimation(Context.AnimationData.CombatParameterName);
        InitailizeSubState();        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        TimeToNonCombatMode();              
    }

    public override void ExitState()
    {
        ResetSubState();
        StopAnimation(Context.AnimationData.CombatParameterName);
    }

    public override void InitailizeSubState()
    {
        if (Context.Controller.isGrounded)
            SetSubState(Factory.Grounded());
        else
        {
            if (Context.IsJumping)
                SetSubState(Factory.Jump());
            else
            {
                if (Context.IsHovering)
                    SetSubState(Factory.Hover());
                else
                    SetSubState(Factory.Fall());
            }
        }
    }

    public override void CheckSwitchStates()
    {
        if (_timeToNonCombat > Context.TIME_TO_NON_COMBAT_MODE)
        {
            _timeToNonCombat = 0;
            SwitchState(Factory.NonCombat());
        }
    }

    private void TimeToNonCombatMode()
    {
        if (!Context.IsLeftArmWeaponInputPressed && !Context.IsRightArmWeaponInputPressed && !Context.IsLeftShoulderWeaponInputPressed && !Context.IsRightShoulderWeaponInputPressed)
            _timeToNonCombat += Time.deltaTime;
        else
            _timeToNonCombat = 0;
    }
}
