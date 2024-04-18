public class Drone_AliveState : BaseState
{
    public Drone_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState?.EnterState();
        Context.Sound.StartEmitter();

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.AP <= 0)
            SwitchState(Provider.GetState(Minion_States.Dead));
    }

    public override void ExitState()
    {
        Context.Sound.StopEmitter();

    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(Minion_States.NonCombat));
    }
}
