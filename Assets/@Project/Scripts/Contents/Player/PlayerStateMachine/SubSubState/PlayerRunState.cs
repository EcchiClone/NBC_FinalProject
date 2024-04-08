using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        StartAnimation(Context.AnimationData.RunParameterName);        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        StopAnimation(Context.AnimationData.RunParameterName);
        Context.StopRun();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMoveInputPressed)
            SwitchState(Factory.Idle());
        if (Context.IsDashInputPressed && Context.CanDash)
            SwitchState(Factory.Dash());
    }
}
