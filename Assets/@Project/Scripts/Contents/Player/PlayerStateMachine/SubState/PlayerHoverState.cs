using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverState : PlayerBaseState
{
    public PlayerHoverState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        InitailizeSubState();
        EnterHover();
        StartAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void UpdateState()
    {
        HandleHovering();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        ExitHover();
        StopAnimation(Context.AnimationData.JumpParameterName);
    }

    public override void InitailizeSubState()
    {
        if (!Context.IsMoveInputPressed)
            SetSubState(Factory.Idle());
        else
        {
            if (!Context.CanJudgeDashing)
                SetSubState(Factory.Dash());
            else
            {
                if (!Context.IsRun)
                    SetSubState(Factory.Walk());
                else
                    SetSubState(Factory.Run());
            }
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

    private void EnterHover()
    {
        Context._currentMovementDirection.y = 0;
        Context.IsHovering = true;
        Context.IsCanHovering = false;
        Context.IsUsingBoost = true;
        Context.Sound.IsHovering = true;
        if (!Context.IsRun)
            Context.Module.CurrentUpper.BoostOnOff(true);
    }

    private void ExitHover()
    {
        Context.IsHovering = false;
        Context.IsUsingBoost = false;
        Context.Sound.IsHovering = false;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Booster_End, Vector3.zero);
        if (!Context.IsRun)
            Context.Module.CurrentUpper.BoostOnOff(false);
    }
}
