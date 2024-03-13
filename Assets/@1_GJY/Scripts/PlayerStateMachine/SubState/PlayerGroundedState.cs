using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory stateFactory) : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        HandleGravity();
        InitailizeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _currentSubState.ExitState();
    }

    public override void InitailizeSubState()
    {
        if (!Context.IsMoveInputPressed)
            SetSubState(Factory.Idle());
        else if (Context.IsMoveInputPressed)
            SetSubState(Factory.Walk());

        _currentSubState.EnterState();
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsJumpInputPressed)
            SwitchState(Factory.Jump());
        else if (!Context.Controller.isGrounded)
            SwitchState(Factory.Fall());
        else if (Context.IsDashInputPressed && Context.CanDash)
            SwitchState(Factory.Dash());
    }

    private void HandleGravity()
    {
        Context._currentMovementDirection.y = -Context.MinDownForceValue;
    }
}
