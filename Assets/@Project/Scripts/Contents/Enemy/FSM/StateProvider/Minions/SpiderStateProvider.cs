using System.Collections.Generic;

public class SpiderStateProvider : BaseStateProvider
{
    public SpiderStateProvider(BaseStateMachine context) 
        : base(context)
    {
        SetState(Spider_States.Alive, new Spider_AliveState(context, this));
        SetState(Spider_States.Dead, new Spider_DeadState(context, this));
        
        SetState(Spider_States.Chasing, new Spider_ChasingState(context, this));
        SetState(Spider_States.Standoff, new Spider_StandoffState(context, this));
        SetState(Spider_States.Run, new Spider_RunState(context, this));
    }
}