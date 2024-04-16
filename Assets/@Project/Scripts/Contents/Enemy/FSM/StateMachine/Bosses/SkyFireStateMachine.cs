public class SkyFireStateMachine : BaseStateMachine
{
    public SkyFireStateMachine(Entity entity) : base(entity)
    {}

    public override void Initialize()
    {
        Provider = new SkyFireStateProvider(this);
        CurrentState = Provider.GetState(SkyFire_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(SkyFire_States.Alive);
        CurrentState.EnterState();
    }
}
