public class SkyFire_AliveState : BaseState
{
    public SkyFire_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState?.EnterState();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Boss_Appear, Context.Entity.transform.position);
        UnityEngine.Debug.Log("보스 생존 입장");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.AP <= 0)
            SwitchState(Provider.GetState(SkyFire_States.Dead));
    }

    public override void ExitState()
    {


    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(SkyFire_States.NonCombat));
    }

}
