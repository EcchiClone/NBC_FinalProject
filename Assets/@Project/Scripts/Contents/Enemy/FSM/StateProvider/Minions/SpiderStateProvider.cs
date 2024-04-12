using System.Collections.Generic;

public class SpiderStateProvider : BaseStateProvider
{
    public SpiderStateProvider(BaseStateMachine context) 
        : base(context)
    {
        SetState(Spider_States.Alive, new Spider_AliveState(context, this));
        SetState(Spider_States.Dead, new Spider_DeadState(context, this));

        SetState(Spider_States.NonCombat, new Spider_NonCombatState(context, this));
        SetState(Spider_States.Combat, new Spider_CombatState(context, this));

        SetState(Spider_States.Idle, new Spider_IdleState(context, this));
        SetState(Spider_States.Chasing, new Spider_ChasingState(context, this));
    }
}
