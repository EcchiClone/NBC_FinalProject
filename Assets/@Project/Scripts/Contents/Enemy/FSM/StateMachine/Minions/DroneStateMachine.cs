public class DroneStateMachine : BaseStateMachine
{
    public DroneStateMachine(Entity entity) : base(entity)
    {
        Provider = new DroneStateProivder(this);
    }

    public override void Initialize()
    {
        
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }

    public override void Activate()
    {
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }
}
