using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    private float _timeToNonCombat;    

    public PlayerCombatState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("컴뱃모드 진입");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        TimeToNonCombatMode();
        HandleGravity();        
    }

    public override void ExitState()
    {
        Debug.Log("컴뱃모드 종료");
    }

    public override void CheckSwitchStates()
    {
        if (_timeToNonCombat > Context.TIME_TO_NON_COMBAT_MODE)
        {
            _timeToNonCombat = 0;
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitailizeSubState()
    {
        if (!Context.IsMoveInputPressed)
            SetSubState(Factory.Idle());
        else if (Context.IsMoveInputPressed)
            SetSubState(Factory.Walk());
    }

    private void TimeToNonCombatMode()
    {
        if (!Context.IsPrimaryWeaponInputPressed)
            _timeToNonCombat += Time.deltaTime;
        else
            _timeToNonCombat = 0;
    }

    private void HandleGravity()
    {
        if (!Context.Controller.isGrounded)
            Context._currentMovementDirection.y += Context.InitialGravity * Time.deltaTime;
    }
}
