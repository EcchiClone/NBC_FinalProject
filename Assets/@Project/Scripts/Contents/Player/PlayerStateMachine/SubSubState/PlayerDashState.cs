using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    private Coroutine _dashRoutine;

    public override void EnterState()
    {            
        StartAnimation(Context.AnimationData.DashParameterName);
        if (_dashRoutine != null)
            CoroutineManager.StopCoroutine(_dashRoutine);
        _dashRoutine = CoroutineManager.StartCoroutine(Context.Co_BoostOn(CheckSwitchStates));
        Context.Sound.IsDashing = true;
    }

    public override void UpdateState()
    {
        HandleGravity();
    }

    public override void ExitState()
    {        
        StopAnimation(Context.AnimationData.DashParameterName);
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMoveInputPressed)
            SwitchState(Factory.Idle());
        else
            SwitchState(Factory.Run());
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
            Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
        else
        {
            if (Context.IsJumping == false)
                Context._currentMovementDirection.y = Context.MIN_GRAVITY_VALUE;
        }
    }
}
