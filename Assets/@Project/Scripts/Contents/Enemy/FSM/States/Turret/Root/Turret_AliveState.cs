public class Turret_AliveState : BaseState
{
    public Turret_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.AP <= 0)
            SwitchState(Provider.GetState(Turret_States.Dead));
    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(Turret_States.NonCombat));
    }

}
