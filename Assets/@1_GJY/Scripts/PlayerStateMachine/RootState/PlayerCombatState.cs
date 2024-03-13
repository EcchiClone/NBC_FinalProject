using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
        HandleGravity();        
    }

    public override void ExitState()
    {
        StopAnimation(Context.AnimationData.CombatParameterName);
    }

    public override void InitailizeSubState()
    {
        if (Context.IsDashing)
            SetSubState(Factory.Dash());
        else
        {
            if (Context.Controller.isGrounded)
                SetSubState(Factory.Grounded());
            else
            {
                if (!Context.Controller.isGrounded)
                {
                    if (Context.IsJumping)
                        SetSubState(Factory.Jump());
                    else
                        SetSubState(Factory.Fall());
                }
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
        if (!Context.IsPrimaryWeaponInputPressed)
            _timeToNonCombat += Time.deltaTime;
        else
            _timeToNonCombat = 0;
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
            Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
    }
}
