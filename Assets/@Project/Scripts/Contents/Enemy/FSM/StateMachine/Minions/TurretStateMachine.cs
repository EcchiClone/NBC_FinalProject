public class TurretStateMachine : BaseStateMachine
{
    public TurretStateMachine(Entity entity) : base(entity)
    {
        Provider = new TurretStateProvider(this);
    }

    public override void Initialize()
    {
        
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }

    public override void Activate()
    {
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }
}
