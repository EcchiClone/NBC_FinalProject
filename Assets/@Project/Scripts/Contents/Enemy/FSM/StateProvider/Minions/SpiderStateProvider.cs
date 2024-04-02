using System.Collections.Generic;

public class SpiderStateProvider : BaseStateProvider
{
    private Dictionary<Minion_States, BaseState> _states = new Dictionary<Minion_States, BaseState>();

    public SpiderStateProvider(BaseStateMachine context) : base(context)
    {
        _states[Minion_States.Alive] = new Spider_AliveState(context, this);
        _states[Minion_States.Dead] = new Spider_AliveState(context, this);

        _states[Minion_States.Chasing] = new Spider_AliveState(context, this);
        _states[Minion_States.Standoff] = new Spider_AliveState(context, this);
        _states[Minion_States.Run] = new Spider_AliveState(context, this);
    }

    // Root
    public BaseState Alive() => _states[Minion_States.Alive];
    public BaseState Dead() => _states[Minion_States.Dead];
    // Sub
    public BaseState Chasing() => _states[Minion_States.Chasing];
    public BaseState Standoff() => _states[Minion_States.Standoff];
    public BaseState Run() => _states[Minion_States.Run];
}
