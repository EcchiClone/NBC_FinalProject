using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        Context.StartDash();
        Context._currentMovementDirection.y = 0;
        StartAnimation(Context.AnimationData.DashParameterName);
        RemoveSubState();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
        if (!Context.IsDashInputPressed && Context.CanJudgeDashing)
            Context.IsDashing = false;
    }

    public override void ExitState()
    {
        Context.StopDash();
        StopAnimation(Context.AnimationData.DashParameterName);
    }

    public override void CheckSwitchStates()
    {
        if(!Context.IsDashInputPressed && !Context.IsDashing)
        {
            if (Context.Controller.isGrounded)
                SwitchState(Factory.Grounded());
            else
                SwitchState(Factory.Fall());
        }
    }

    private void RemoveSubState()
    {
        _currentSubState = null;
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
            Context._currentMovementDirection.y += -Context.MinDownForceValue * Time.deltaTime;
        else
            Context._currentMovementDirection.y = -Context.MinDownForceValue;
    }
}
