using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    : base(currentContext, enemyStateFactory)
    { }

    public override void CheckSwitchStates(){}

    public override void EnterState(){}

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void UpdateState(){}
}
