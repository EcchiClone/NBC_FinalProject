using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsMoveInputPressed)
            SwitchState(Factory.Walk());
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
        {
            if(!Context.IsHovering)
                Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
        }            
        else
        {
            if(Context.IsJumping == false)
                Context._currentMovementDirection.y = Context.MIN_GRAVITY_VALUE;
        }            
    }
}
