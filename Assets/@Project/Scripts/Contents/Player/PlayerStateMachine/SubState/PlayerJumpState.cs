using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        InitailizeSubState();
        Context.IsJumping = true;
        StartAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.IsJumping = false;
        StopAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void InitailizeSubState()
    {        
        HandleJump();
    }

    public override void CheckSwitchStates()
    {
        if (Context.Controller.isGrounded)
            SwitchState(Factory.Grounded());
        else if (Context.IsDashInputPressed && Context.CanDash)
            SwitchState(Factory.Dash());
    }

    private void HandleJump()
    {        
        Context._currentMovementDirection.y = Context.Module.ModuleStatus.JumpPower;
    }

    private void HandleGravity()
    {
        Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
    }
}
