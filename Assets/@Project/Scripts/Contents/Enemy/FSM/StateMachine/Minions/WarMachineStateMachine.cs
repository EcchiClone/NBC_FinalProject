public class WarMachineStateMachine : BaseStateMachine
{
    public WarMachineStateMachine(Entity entity) : base(entity)
    {
        Provider = new WarMachineStateProvider(this);
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
