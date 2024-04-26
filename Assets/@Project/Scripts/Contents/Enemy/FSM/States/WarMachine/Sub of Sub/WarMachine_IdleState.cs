using UnityEngine;

public class WarMachine_IdleState : BaseState
{
    public WarMachine_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.SetStopDistance(0f);

        Debug.Log("기본상태 진입");
    }

    public override void UpdateState()
    {

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Entity.Controller.SetStopDistance();
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Stat.cognizanceRange >= distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Chasing));
        }
    }

    public override void InitializeSubState() { }
}
