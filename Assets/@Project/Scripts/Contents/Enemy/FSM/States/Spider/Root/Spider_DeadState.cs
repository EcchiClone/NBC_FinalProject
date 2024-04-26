public class Spider_DeadState : BaseState
{
    public Spider_DeadState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Managers.Pool.GetPooler(PoolingType.Enemy).SpawnFromPool("EnemyExplosion01", _entityTransform.position);
        Context.Entity.gameObject.SetActive(false);
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
