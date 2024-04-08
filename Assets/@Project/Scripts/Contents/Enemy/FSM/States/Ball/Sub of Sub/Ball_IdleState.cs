using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_IdleState : BaseState
{
    private Transform _entityTransform;
    private Transform _targetTransform;

    public Ball_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;
    }
    public override void EnterState()
    {
        Context.Entity.Controller.Stop();
    }

    public override void CheckSwitchStates()
    {
        // 상호간 거리가 Chasing Distance 이하면 추적으로 변경
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.chasingDistance >= distance)
        {
            SwitchState(Context.Provider.GetState(Ball_States.Chasing));
        }
    }    

    public override void ExitState()
    {
       
    }

    public override void InitializeSubState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
