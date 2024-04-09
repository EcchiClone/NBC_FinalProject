using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        StartAnimation(Context.AnimationData.WalkParameterHash);
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        StopAnimation(Context.AnimationData.WalkParameterHash);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMoveInputPressed)
            SwitchState(Factory.Idle());
        if (Context.IsDashInputPressed)
            SwitchState(Factory.Dash());
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
        {
            if (!Context.IsHovering)
                Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
        }
        else
        {
            if (Context.IsJumping == false)
                Context._currentMovementDirection.y = Context.MIN_GRAVITY_VALUE;
        }
    }
}
