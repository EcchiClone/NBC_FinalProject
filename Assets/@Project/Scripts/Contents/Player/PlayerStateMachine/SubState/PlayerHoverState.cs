using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverState : PlayerBaseState
{
    public PlayerHoverState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        Context.IsHovering = true;        
        Context.StartHovering();
        StartAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void UpdateState()
    {
        HandleJumpPack();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.IsHovering = false;
        Context.StopHovering();
        StopAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsJumpInputPressed)
            SwitchState(Factory.Fall());
    }

    private void HandleJumpPack()
    {
        if (Context.IsJumpInputPressed)
            Context._currentMovementDirection.y = Mathf.Min(Context._currentMovementDirection.y + Context.Module.ModuleStatus.Hovering, Context.MAX_HOVER_VALUE);
    }
}
