using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory stateFactory) : base(currentContext, stateFactory) { }

    public override void EnterState()
    {        
        Context.IsCanHovering = false;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Land, Context.transform.position);
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
        if (Context._currentMovementInput.x == 0 && Context._currentMovementInput.y == 0)
            SetSubState(Factory.Idle());
        else
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
