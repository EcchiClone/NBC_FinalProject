using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNonCombatState : PlayerBaseState
{
    public PlayerNonCombatState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        StateType = RootStateType.NonCombat;
        StartAnimation(Context.AnimationData.NonCombatParameterName);
        Context.ResetWeaponTilt();
        InitailizeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.StopResetWeaponTilt(); // 정지 안 되는 버그있음.
        StopAnimation(Context.AnimationData.NonCombatParameterName);
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
        if (Context.IsPrimaryWeaponInputPressed || Context.IsSecondaryWeaponInputPressed)
            SwitchState(Factory.Combat());
    }
}
