using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverState : PlayerBaseState
{
    public PlayerHoverState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        InitailizeSubState();
        Context.IsHovering = true;
        Context.IsUsingBoost = true;
        if (!Context.IsRun)
            Context.Module.CurrentUpper.BoostOnOff(true);
        StartAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void UpdateState()
    {
        HandleHovering();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.IsHovering = false;
        Context.IsUsingBoost = false;
        if (!Context.IsRun)
            Context.Module.CurrentUpper.BoostOnOff(false);
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
        if (!Context.IsJumpInputPressed || Context.Module.ModuleStatus.CurrentBooster <= 0)
            SwitchState(Factory.Fall());
    }

    private void HandleHovering()
    {        
        Context.Module.ModuleStatus.Hovering(() =>
        Context._currentMovementDirection.y = Mathf.Min(Context._currentMovementDirection.y + Context.Module.ModuleStatus.VTOL * Time.deltaTime, Context.MAX_HOVER_VALUE));        
    }
}
