using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        StartAnimation(Context.AnimationData.JumpParameterName);
        Context.IsCanHovering = true;
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        StopAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void CheckSwitchStates()
    {
        if (Context.Controller.isGrounded)
            SwitchState(Factory.Grounded());
        else if (Context.IsDashInputPressed && Context.CanDash)
            SwitchState(Factory.Dash());
        else if (Context.IsJumpInputPressed)
            SwitchState(Factory.Hover());
    }

    private void HandleGravity()
    {
        Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
    }
}
