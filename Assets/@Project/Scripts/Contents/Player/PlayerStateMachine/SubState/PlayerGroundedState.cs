using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory stateFactory) : base(currentContext, stateFactory) { }

    public override void EnterState()
    {        
        Context.IsCanHovering = false;
        if (Context.IsRun)
        {
            StopAnimation(Context.AnimationData.DashParameterName);
            Context.Module.CurrentLower.FootSparksOnOff(true);
            Context.Sound.IsDragging = true;
        }            
        InitailizeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        if (Context.IsRun)
        {
            Context.Sound.IsDragging = false;
            Context.Module.CurrentLower.FootSparksOnOff(false);        
        }
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
        if (Context.IsJumpInputPressed)
            SwitchState(Factory.Jump());
        else if (!Context.Controller.isGrounded)
            SwitchState(Factory.Fall());        
    }
}
