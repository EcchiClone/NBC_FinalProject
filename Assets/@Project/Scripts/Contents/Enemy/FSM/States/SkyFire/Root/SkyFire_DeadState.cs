public class SkyFire_DeadState : BaseState
{
    public SkyFire_DeadState(BaseStateMachine context, BaseStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {        
        Context.Entity.enemyPhaseStarter.isShooting = false;
        Context.Entity.gameObject.SetActive(false);
        UnityEngine.Debug.Log("보스 죽음 입장");
    }
    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }

}
