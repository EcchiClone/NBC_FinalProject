using UnityEngine;

public class WarMachine_NonCombatState : BaseState
{
    public WarMachine_NonCombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState?.EnterState();
        Debug.Log("비전투 진입");
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
        if (Context.Entity.Stat.stopDistance > distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Combat));
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(Minion_States.Idle));
    }
}
