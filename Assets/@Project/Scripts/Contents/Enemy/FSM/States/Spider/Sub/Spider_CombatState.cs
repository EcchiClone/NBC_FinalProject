using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_CombatState : BaseState
{
    private Transform _entityTransform;
    private Transform _targetTransform;

    public Spider_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;
    }    

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        // 총 쏘는 중이라면 
        Debug.Log("공격");

        Context.Entity.enemyPhaseStarter.StartPhase(0, 1, true);
        Context.Entity.enemyPhaseStarter.StartPhase(0, 2, true);

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
            SwitchState(Context.Provider.GetState(Spider_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }    
}
