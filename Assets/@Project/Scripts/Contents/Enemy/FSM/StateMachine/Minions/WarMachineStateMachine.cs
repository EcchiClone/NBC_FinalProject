public class WarMachineStateMachine : BaseStateMachine
{
    public WarMachineStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        Provider = new WarMachineStateProvider(this);
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }
}
