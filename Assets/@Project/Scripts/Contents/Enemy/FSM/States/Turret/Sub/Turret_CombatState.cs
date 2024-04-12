using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_CombatState : BaseState
{
    private float _passedTime;
    private float _attackInterval;
    public Turret_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        _attackInterval = Context.Entity.Data.attackInterval;
    }

    public override void EnterState()
    {
        InitializeSubState();

        _passedTime = 0;
    }

    public override void UpdateState()
    {
        _passedTime += Time.deltaTime;
        if (_passedTime > _attackInterval)
        {
            Context.Entity.enemyPhaseStarter.StartPhase(0, 1, true);
            Context.Entity.enemyPhaseStarter.StartPhase(0, 2, true);
            _passedTime = 0;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.stopDistance <= distance)
        {
            SwitchState(Context.Provider.GetState(Turret_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }
}
