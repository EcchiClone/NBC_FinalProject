
using UnityEngine;

public class Ball_NonCombatState : BaseState
{

    public Ball_NonCombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }
    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState?.EnterState();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        // 목표물과 거리 차 Stop Distance 이하면 컴뱃으로 변경.
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if(Context.Entity.Stat.stopDistance > distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Combat));
        }
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        // Idle 설정
        SetSubState(Provider.GetState(Minion_States.Idle));
    }

}
