public class TurretStateMachine : BaseStateMachine
{
    public TurretStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        Provider = new TurretStateProvider(this);
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }
}
