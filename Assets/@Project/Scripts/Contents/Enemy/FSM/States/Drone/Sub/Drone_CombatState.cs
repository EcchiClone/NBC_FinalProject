using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_CombatState : BaseState
{
    private float _passedTime;
    private float _attackInterval;

    public Drone_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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
        _passedTime += Time.deltaTime;
        if (_passedTime > _attackInterval)
        {
            if (!CheckObstacle())
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Drone_Shot, _entityTransform.position);
                Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 1, true);
                Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 2, true);
            }
                
            _passedTime = 0;
        }

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
