using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandoffState : EnemyBaseState
{
    /*private float _attackDuration = 2.0f; // 공격 시간
    private float _attackInterval = 3.0f; // 공격 간격
    private float _passedTime;*/

    private WaitForSeconds _attackDuration = new WaitForSeconds(3.0f); // 공격 시간
    private WaitForSeconds _attackInterval = new WaitForSeconds(2f); // 공격 간격
    private WaitForSeconds _restDuration = new WaitForSeconds(2.0f); // 쉬는 시간

    public EnemyStandoffState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) 
        : base(currentContext, enemyStateFactory)
    {
    }

    public override void EnterState()
    {
        //애니메이션 재생 등 
        Debug.Log("StandoffState");

        _ctx.StartCoroutine(AttackRoutine());

    }

    public override void UpdateState()
    {
        /*_passedTime += Time.deltaTime;

        if (_passedTime >= _attackInterval)
        {
            if (_passedTime <= _attackInterval + _attackDuration)
            {
                Attack(); // 공격 시간 동안 Attack 호출
            }
            else
            {
                _ctx.GizmoColor = Color.green;
            }

            if (_passedTime >= _attackInterval + _attackDuration)// 타이머 초기화
            {
                _passedTime = 0f;
            }
        }*/

        LookAtTarget();

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.StopAllCoroutines();
    }


    public override void InitializeSubState()
    {
        
    }
    public override void CheckSwitchStates()
    {
        if (_ctx.IsChasing)
            SwitchState(_factory.Chasing());
        else if (_ctx.IsRun)
            SwitchState(_factory.Run());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            // 3초간 공격
            _ctx.StartCoroutine(Attack());
            yield return _attackDuration;

            // 2초 동안 쉬기
            yield return _restDuration;
        }
    }

    private IEnumerator Attack()
    {

        while (true)
        {
            _ctx.GizmoColor = Color.red;
            // TODO : 무언가 쏘는 함수 호출
            foreach (var patternHierarchy in _ctx.currentPhase.hierarchicalPatterns)
            {
                EnemyBulletGenerator.instance.StartPatternHierarchy(patternHierarchy, _ctx.currentPhase.cycleTime, _ctx.gameObject);
            }

            // 0.5초 대기
            yield return _attackInterval;
        }
    }
}
