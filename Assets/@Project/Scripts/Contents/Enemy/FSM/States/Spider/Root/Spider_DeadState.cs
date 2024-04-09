using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_DeadState : BaseState // TODO
{
    public Spider_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.SetDestination(Context.Entity.transform.position);
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
