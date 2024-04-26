public class BallStateMachine : BaseStateMachine
{
    public BallStateMachine(Entity boss) : base(boss)
    {
        Provider = new BallStateProvider(this);
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
