public class Ball_AliveState : BaseState
{
    public Ball_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
       _currentSubState?.EnterState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.AP <= 0)
            SwitchState(Provider.GetState(Ball_States.Dead));
    }

    public override void ExitState()
    {


    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(Ball_States.NonCombat));
    }

}
