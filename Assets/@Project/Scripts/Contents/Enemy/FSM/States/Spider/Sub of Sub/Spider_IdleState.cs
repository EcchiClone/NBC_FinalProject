using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IdleState : BaseState
{
    private Transform _entityTransform;
    private Transform _targetTransform;

    private Vector3 _originPoint;

    public Spider_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        _originPoint = _entityTransform.position;
        Context.Entity.Controller.SetStopDistance(0f);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Entity.Controller.SetStopDistance();
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
