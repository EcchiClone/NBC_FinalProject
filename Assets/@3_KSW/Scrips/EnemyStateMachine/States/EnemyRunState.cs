using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyBaseState
{
    private float _calculateInterval = 0.2f;
    private float _lastRecalculateTime;
    private Vector3 _directionToTarget;

    public EnemyRunState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
        : base(currentContext, enemyStateFactory)
    { }


    public override void EnterState()
    {
        //애니메이션 재생 등
        Debug.Log("RunState");

        _lastRecalculateTime = Time.time;

        //도망칠 땐 움직여야해서 에이전트 값 조정 필요
        _ctx.Nma.stoppingDistance = 0.1f;
    }

    public override void UpdateState()
    {

        if (_calculateInterval < Time.time - _lastRecalculateTime)
        {
            _directionToTarget = (_ctx.target.position - _ctx.transform.position).normalized;
            Vector3 runDirection = -_directionToTarget; // 반대 방향으로 설정
            _ctx.Nma.SetDestination(_ctx.transform.position + runDirection * 5f);
            _lastRecalculateTime = Time.time;
        }

        LookAtTarget();

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.Nma.ResetPath();
        _ctx.Nma.stoppingDistance = _ctx.StoppingDistance;
    }

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if(_ctx.IsStandoff)
            SwitchState(_factory.Standoff());
        else if (_ctx.IsChasing)
            SwitchState(_factory.Chasing());
    }

}
