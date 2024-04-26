using UnityEngine;

public class Drone_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;

    public Drone_ChasingState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;

        chasingInterval = Context.Entity.Stat.chasingInterval;
    }
    public override void EnterState()
    {
        passedTime = 0f;

        Context.Entity.Controller.IsChasing = true;

        Context.Sound.StartEmitter();
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            if (!CheckObstacle())
                Context.Entity.Controller.SetDestination(_targetTransform.position);
            else
                Context.Entity.Controller.SetDestination(_entityTransform.position);

            passedTime = 0f;
        }
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        Vector3 entity = new Vector3(_entityTransform.position.x, 0, _entityTransform.position.z);
        Vector3 target = new Vector3(_targetTransform.position.x, 0, _targetTransform.position.z);

        float distance = Vector3.Distance(entity, target);
        if (Context.Entity.Stat.cognizanceRange < distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Idle));
        }
    }

    public override void ExitState()
    {
        Context.Entity.Controller.IsChasing = false;

        Context.Sound.StopEmitter();
    }

    public override void InitializeSubState()
    {
    }

    
}
