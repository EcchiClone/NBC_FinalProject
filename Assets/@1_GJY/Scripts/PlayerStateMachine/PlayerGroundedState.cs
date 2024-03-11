using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory stateFactory) : base(currentContext, stateFactory) 
    {
        InitailizeSubState();
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context._currentMovementDirection.y = -Context.MinDownForceValue;
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();        
    }

    public override void ExitState()
    {
        
    }

    public override void InitailizeSubState()
    {
        if (!Context.IsMoveInputPressed)
            SetSubState(Factory.Idle());
        else if (Context.IsMoveInputPressed)
            SetSubState(Factory.Walk());
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsJumpInputPressed)
            SwitchState(Factory.Jump());

        else if (!Context.Controller.isGrounded)
            SwitchState(Factory.Fall());

        else if (Context.IsPrimaryWeaponInputPressed || Context.IsSecondaryWeaponInputPressed)
            SwitchState(Factory.Combat());
    }
}
