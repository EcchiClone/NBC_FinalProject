using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {        
        StartAnimation(Context.AnimationData.DashParameterName);
        Context.StartRunAfterDash();
    }

    public override void UpdateState()
    {
        HandleGravity();        
        CheckSwitchStates();        
    }

    public override void ExitState()
    {        
        StopAnimation(Context.AnimationData.DashParameterName);
    }

    public override void CheckSwitchStates()
    {
        if (Context.CanJudgeDashing)
        {
            if(!Context.IsMoveInputPressed)
                SwitchState(Factory.Idle());
            else
                SwitchState(Factory.Run());
        }
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
            Context._currentMovementDirection.y += Context.MIN_GRAVITY_VALUE * Time.deltaTime;
        else
        {
            if (Context.IsJumping == false)
                Context._currentMovementDirection.y = Context.MIN_GRAVITY_VALUE;
        }
    }
}
