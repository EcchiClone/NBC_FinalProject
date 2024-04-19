public class DroneStateMachine : BaseStateMachine
{
    public DroneStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        Provider = new DroneStateProivder(this);
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }
}
