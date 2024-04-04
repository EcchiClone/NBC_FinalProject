using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;

    public Spider_ChasingState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        chasingInterval = Context.Entity.Data.chasingInterval;
    }
    public override void EnterState()
    {
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
    }
    
    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void CheckSwitchStates()
    {
        if (false == !Context.Entity.Controller.IsMoving)
            SwitchState(Provider.GetState(Spider_States.Standoff));
    }



    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

}
