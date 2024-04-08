using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        Context.IsJumping = true;
        HandleJump();
        InitailizeSubState();
        StartAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.IsJumping = false;
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

    private void HandleJump()
    {
        Context._currentMovementDirection.y = 0;        
        Context._currentMovementDirection.y = Context.Module.ModuleStatus.JumpPower;
    }
}
