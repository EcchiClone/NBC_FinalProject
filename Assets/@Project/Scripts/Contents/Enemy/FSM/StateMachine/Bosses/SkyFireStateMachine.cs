public class SkyFireStateMachine : BaseStateMachine
{
    public SkyFireStateMachine(Entity entity) : base(entity)
    {
        Provider = new SkyFireStateProvider(this);
    }

    public override void Initialize()
    {
        
        CurrentState = Provider.GetState(SkyFire_States.Alive);
        CurrentState.EnterState();
    }

    public override void Activate()
    {
        CurrentState = Provider.GetState(SkyFire_States.Alive);
        CurrentState.EnterState();
    }
}
