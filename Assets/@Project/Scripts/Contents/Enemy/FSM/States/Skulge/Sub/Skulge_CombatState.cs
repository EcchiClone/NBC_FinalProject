using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skulge_CombatState : BaseState
{
    private float _passedTime;
    private float _attackInterval;

    public Skulge_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        _attackInterval = Context.Entity.Data.attackInterval;
    }

    public override void EnterState()
    {
        InitializeSubState();

        _passedTime = 0;

        Context.Entity.Controller.IsChasing = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Entity.Controller.IsChasing = false;
    }

    public override void CheckSwitchStates()
    {
        Vector3 entity = new Vector3(_entityTransform.position.x, 0, _entityTransform.position.z);
        Vector3 target = new Vector3(_targetTransform.position.x, 0, _targetTransform.position.z);

        float distance = Vector3.Distance(entity, target);
        if (Context.Entity.Data.stopDistance <= distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }
}
