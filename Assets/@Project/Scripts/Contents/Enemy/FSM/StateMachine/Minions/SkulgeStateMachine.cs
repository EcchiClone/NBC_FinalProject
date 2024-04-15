public class SkulgeStateMachine : BaseStateMachine
{
    public SkulgeStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        Provider = new SkulgeStateProivder(this);
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }
}
