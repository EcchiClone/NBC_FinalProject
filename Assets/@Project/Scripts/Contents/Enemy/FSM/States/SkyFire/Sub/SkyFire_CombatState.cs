using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_CombatState : BaseState
{
    public SkyFire_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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
        SetSubState(Provider.GetState(SkyFire_States.Phase1));
    }
}