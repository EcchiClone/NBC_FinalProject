using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_IdleState : BaseState
{
    public Turret_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.IsMoving = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Entity.Controller.IsMoving = true;
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState() { }
}
