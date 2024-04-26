using UnityEngine;

public class Ball_IdleState : BaseState
{
    public Ball_IdleState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }
    public override void EnterState()
    {
        Context.Entity.Controller.Stop();
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.cognizanceRange >= distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.Chasing));
        }
    }    

    public override void ExitState()
    {       
    }

    public override void InitializeSubState()
    {       
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
