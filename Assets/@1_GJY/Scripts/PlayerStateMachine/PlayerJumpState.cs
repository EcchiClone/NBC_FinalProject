using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) 
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {        
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        Context.IsJumping = false;
    }

    public override void InitailizeSubState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if (Context.Controller.isGrounded)
            SwitchState(Factory.Grounded());
    }
    
    private void HandleJump()
    {
        Context.IsJumping = true;
        Context._currentMovementDirection.y = Context.JumpPower;
    }

    private void HandleGravity()
    {
        Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
    }
}
