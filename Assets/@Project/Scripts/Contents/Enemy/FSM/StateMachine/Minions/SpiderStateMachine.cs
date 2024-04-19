public class SpiderStateMachine : BaseStateMachine
{
    public SpiderStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        Provider = new SpiderStateProvider(this);
        CurrentState = Provider.GetState(Spider_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Spider_States.Alive);
        CurrentState.EnterState();
    }
}
