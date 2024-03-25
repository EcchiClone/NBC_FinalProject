using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private float _Interval = 0.2f;
    private float _passedTime;

    public EnemyChasingState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) 
        : base(currentContext, enemyStateFactory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("ChasingState");
    }

    public override void UpdateState()
    {
        if(_Interval < _passedTime)
        {
            _ctx.Nma.SetDestination(_ctx.Target.position);
            _passedTime = 0f;
        }
        _passedTime += Time.deltaTime;

        LookAtTarget();

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.Nma.ResetPath();
    }
    

    public override void InitializeSubState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        if(_ctx.IsStandoff)
            SwitchState(_factory.Standoff());
        else if (_ctx.IsRun)
            SwitchState(_factory.Run());
    }
}