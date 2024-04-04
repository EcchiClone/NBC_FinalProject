using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_StandoffState : BaseState
{
    public Spider_StandoffState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (true == Context.Entity.Controller.IsMoving)
            SwitchState(Provider.GetState(Spider_States.Standoff));
    }

    public override void InitializeSubState()
    {
    }
}
