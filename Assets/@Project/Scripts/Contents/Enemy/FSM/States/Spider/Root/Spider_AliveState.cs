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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Others_Appear, Context.Entity.transform.position);
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Enemy_Down, Context.Entity.transform.position);
    }

    public override void InitializeSubState() // 처음 적용할 상태
    {
        SetSubState(Provider.GetState(Spider_States.NonCombat));
    }

}
