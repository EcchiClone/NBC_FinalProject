public class Spider_AliveState : BaseState
{
    public Spider_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
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
            SwitchState(Provider.GetState(Spider_States.Dead));
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState() // 처음 적용할 상태
    {
        SetSubState(Provider.GetState(Spider_States.NonCombat));
    }

}
