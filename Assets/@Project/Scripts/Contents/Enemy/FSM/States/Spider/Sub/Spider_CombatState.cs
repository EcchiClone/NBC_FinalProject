using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_CombatState : BaseState
{
    private Transform _entityTransform;
    private Transform _targetTransform;

    private float passedTime;

    public Spider_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;
    }    

    public override void EnterState()
    {
        InitializeSubState();        

        passedTime = 0;
    }

    public override void UpdateState()
    {
        // 총 쏘는 중이라면 
        Debug.Log("공격");

        passedTime += Time.deltaTime;
        if (passedTime > Context.Entity.Data.attackInterval)
        {
            Context.Entity.enemyPhaseStarter.StartPhase(0, 1, true);
            Context.Entity.enemyPhaseStarter.StartPhase(0, 2, true);
            passedTime = 0;
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
            SwitchState(Context.Provider.GetState(Spider_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }    
}
