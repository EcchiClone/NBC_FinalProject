using UnityEngine;

public class Turret_NonCombatState : BaseState
{
    public Turret_NonCombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.stopDistance > distance)
        {
            SwitchState(Context.Provider.GetState(Turret_States.Combat));
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(Turret_States.Idle));
    }
}
