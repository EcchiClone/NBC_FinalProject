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

public abstract class BaseStateProvider
{
    protected BaseStateMachine _context;
    public BaseStateProvider(BaseStateMachine context)
    {
        _context = context;
    }


    // TODO

}
