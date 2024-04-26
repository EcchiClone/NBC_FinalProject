using UnityEngine;

public class Ball_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;

    public Ball_ChasingState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;

        chasingInterval = Context.Entity.Stat.chasingInterval;
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;
    }

    public override void EnterState()
    {             
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(_targetTransform.position);
        Context.Sound.StartEmitter();
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Vector3 dest = Context.Entity.Target.position;
            dest.y = 0;
            Context.Entity.Controller.SetDestination(dest);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        // 거리 멀어지면 다시 Idle로
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Stat.cognizanceRange < distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Idle));
        }
    }

    public override void ExitState()
    {
        Context.Sound.StopEmitter();
    }

    public override void InitializeSubState()
    {
    }
}
