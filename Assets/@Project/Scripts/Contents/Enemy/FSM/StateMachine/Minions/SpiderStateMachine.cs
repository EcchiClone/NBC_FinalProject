public class SpiderStateMachine : BaseStateMachine
{
    public SpiderStateMachine(Entity entity) : base(entity)
    {
        Provider = new SpiderStateProvider(this);
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
