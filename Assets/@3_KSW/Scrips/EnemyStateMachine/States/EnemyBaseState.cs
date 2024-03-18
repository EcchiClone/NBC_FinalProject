using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected bool _isRootState = false;
    protected EnemyStateMachine _ctx;
    protected EnemyStateFactory _factory;
    protected EnemyBaseState _currentSuperState;
    protected EnemyBaseState _currentSubState;

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    {
        _ctx = currentContext;
        _factory = enemyStateFactory;
    }


    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates(); // 상태 나타내는 bool변수 두고 어떤 상태로 바꿀지 결정
    public abstract void InitializeSubState();
    public void UpdateStates() 
    {
        UpdateState();
        if (_currentSubState != null)
            _currentSubState.UpdateStates();

    }

    public void ExitStates() // 보류
    {
        ExitState();
        if (_currentSubState != null)
            _currentSubState.ExitStates();
    }

    protected void SwitchState(EnemyBaseState newState) 
    { 
        ExitState();

        newState.EnterState();

        if(_isRootState) // 루트라면(Grounded나 Fall이라면) 루트끼리 바뀌게
        {
            _ctx.CurrentState = newState;
        }
        else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    public void SetSuperState(EnemyBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    public void SetSubState(EnemyBaseState newSubState) 
    { 
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }


    protected void LookAtTarget()
    {
        Vector3 direction = (_ctx.Target.position - _ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, lookRotation, 180 * Time.deltaTime);
        // TODO : 로테이션 스피드 10 -> 커스터마이징 가능하게
    }
}