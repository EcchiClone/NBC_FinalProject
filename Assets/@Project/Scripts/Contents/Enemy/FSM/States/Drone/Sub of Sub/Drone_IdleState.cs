using UnityEngine;

public class Drone_IdleState : BaseState
{
    public Drone_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Context.Entity.Controller.SetStopDistance(0f);
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
        Vector3 entity = new Vector3(_entityTransform.position.x, 0, _entityTransform.position.z);
        Vector3 target = new Vector3(_targetTransform.position.x, 0, _targetTransform.position.z);

        float distance = Vector3.Distance(entity, target);
        if (Context.Entity.Data.cognizanceRange >= distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Chasing));
        }
    }

    public override void InitializeSubState() { }
}
