
using UnityEngine;

public class Ball_NonCombatState : BaseState
{
    private Transform _entityTransform;
    private Transform _targetTransform;

    public Ball_NonCombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }
    public override void EnterState()
    {
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;

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
        if(Context.Entity.Data.stopDistance > distance)
        {
            SwitchState(Context.Provider.GetState(Ball_States.Combat));
        }
    }

    public override void ExitState()
    {
        // 컨트롤러에서 스탑
        Context.Entity.Controller.Stop();
    }

    public override void InitializeSubState()
    {
        // Idle 설정
        SetSubState(Provider.GetState(Ball_States.Idle));
    }

}
