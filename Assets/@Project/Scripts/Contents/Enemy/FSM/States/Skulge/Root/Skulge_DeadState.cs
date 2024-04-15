using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skulge_DeadState : BaseState
{
    public Skulge_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.Entity.gameObject.SetActive(false);
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }
}
