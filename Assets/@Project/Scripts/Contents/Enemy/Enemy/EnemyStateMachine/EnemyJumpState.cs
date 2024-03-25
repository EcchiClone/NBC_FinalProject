using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpState : EnemyBaseState
{
    public EnemyJumpState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
        : base(currentContext, enemyStateFactory)
    { }


    public override void EnterState()
    {
        Debug.Log("JumpState");
        // 이 상태에 들어왔을 때 무언가 실행할 함수를 호출한다.
    }
    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        
    }
}
