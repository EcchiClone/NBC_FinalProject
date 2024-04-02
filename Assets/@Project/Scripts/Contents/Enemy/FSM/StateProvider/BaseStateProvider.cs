using System.Collections.Generic;

public enum Boss_States
{
    Alive,
    Dead,
    
    Chasing,

    Phase1,
    Phase2,
}

public enum Minion_States
{
    Alive,
    Dead,

    Chasing,
    Standoff,
    Run,
}

public abstract class BaseStateProvider<TEnum>
{
    protected BaseStateMachine _context;
    protected Dictionary<TEnum, BaseState> States { get; set; } = new Dictionary<TEnum, BaseState>();
    public BaseStateProvider(BaseStateMachine context)
    {
        _context = context;
    }

    public BaseState GetState(TEnum stateEnum)
    {
        if(States.ContainsKey(stateEnum))
            return States[stateEnum];
        else
            throw new KeyNotFoundException($"{stateEnum.ToString()} State not found");
    }

    protected void AddState()
    {

    }

    // TODO

}
