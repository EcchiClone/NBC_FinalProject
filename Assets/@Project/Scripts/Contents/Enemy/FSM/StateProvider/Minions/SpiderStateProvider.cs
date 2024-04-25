public class SpiderStateProvider : BaseStateProvider
{
    public SpiderStateProvider(BaseStateMachine context) 
        : base(context)
    {
        SetState(Minion_States.Alive, new Spider_AliveState(context, this));
        SetState(Minion_States.Dead, new Spider_DeadState(context, this));

        SetState(Minion_States.NonCombat, new Spider_NonCombatState(context, this));
        SetState(Minion_States.Combat, new Spider_CombatState(context, this));

        SetState(Minion_States.Idle, new Spider_IdleState(context, this));
        SetState(Minion_States.Chasing, new Spider_ChasingState(context, this));
    }
}
