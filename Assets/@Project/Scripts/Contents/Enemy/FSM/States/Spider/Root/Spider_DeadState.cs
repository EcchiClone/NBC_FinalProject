using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_DeadState : BaseState // TODO
{
    public Spider_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
