using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
        :base(currentContext, enemyStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }


    public override void EnterState()
    {
        Debug.Log("GroundState");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }
    public override void InitializeSubState()
    {
        if (_ctx.IsChasing)
            SetSubState(_factory.Chasing());
        else if (_ctx.IsStandoff)
            SetSubState(_factory.Standoff());
        else if(_ctx.IsRun)
            SetSubState(_factory.Run());
    }
    public override void CheckSwitchStates()
    {
    }
}
