using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        InitailizeSubState();
        StartAnimation(Context.AnimationData.JumpParameterName);
        Context.IsCanHovering = true;
    }

    public override void UpdateState()
    {        
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        StopAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void InitailizeSubState()
    {
        if (!Context.IsMoveInputPressed)
            SetSubState(Factory.Idle());
        else if (Context.IsMoveInputPressed)
        {
            if (!Context.IsRun)
                SetSubState(Factory.Walk());
            else
                SetSubState(Factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Context.Controller.isGrounded)
            SwitchState(Factory.Grounded());        
        else if (Context.IsJumpInputPressed && Context.IsCanHovering)
            SwitchState(Factory.Hover());
    }
}
