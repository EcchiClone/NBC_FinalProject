using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_CombatState : BaseState
{
    private float _passedTime;
    private float _attackInterval;

    public Spider_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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
            if (!CheckObstacle())
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Spider_Shot, _entityTransform.position);
                Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 1, true);
                Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 2, true);
            }
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
            SwitchState(Context.Provider.GetState(Spider_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }    
}
