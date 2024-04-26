using UnityEngine;

public class WarMachine_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;


    public WarMachine_ChasingState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;

        chasingInterval = Context.Entity.Stat.chasingInterval;
    }
    public override void EnterState()
    {
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(_targetTransform.position);
        //Context.Sound.StartEmitter();

        Debug.Log("추격 진입");
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Context.Entity.Controller.SetDestination(_targetTransform.position);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Stat.cognizanceRange < distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Idle));
        }
    }

    public override void ExitState()
    {
        //Context.Sound.StopEmitter();
    }

    public override void InitializeSubState()
    {
    }
}
