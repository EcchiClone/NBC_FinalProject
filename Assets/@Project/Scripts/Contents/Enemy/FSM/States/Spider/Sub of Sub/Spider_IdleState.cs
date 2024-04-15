using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IdleState : BaseState
{
    private float patrolInterval;
    private float passedTime;

    public Spider_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.SetStopDistance(0f);

        passedTime = 0f;
        patrolInterval = 5f;
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= patrolInterval)
        {
            float x = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            Vector3 destination = new Vector3(x, _entityTransform.position.y, z);
            destination.Normalize();
            destination *= Random.Range(1f, Context.Entity.Data.patrolDistance + 1);
            destination += _entityTransform.position;

            Context.Entity.Controller.SetDestination(destination);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Entity.Controller.SetStopDistance();
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.cognizanceRange >= distance)
        {
            SwitchState(Context.Provider.GetState(Spider_States.Chasing));
        }
    }    

    public override void InitializeSubState() {}    
}
