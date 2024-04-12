using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IdleState : BaseState
{
    public Spider_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.SetDestination(_entityTransform.position);
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
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.cognizanceRange >= distance)
        {
            SwitchState(Context.Provider.GetState(Spider_States.Chasing));
        }
    }    

    public override void InitializeSubState() {}    
}
