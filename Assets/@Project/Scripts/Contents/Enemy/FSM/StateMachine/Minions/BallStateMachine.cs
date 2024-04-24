public class BallStateMachine : BaseStateMachine
{
    public BallStateMachine(Entity boss) : base(boss)
    {
    }

    public override void Initialize()
    {
        Provider = new BallStateProvider(this);
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();

    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Minion_States.Alive);
        CurrentState.EnterState();
    }
}
