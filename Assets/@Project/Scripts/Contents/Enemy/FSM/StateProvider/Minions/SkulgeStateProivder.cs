public class SkulgeStateProivder : BaseStateProvider
{
    public SkulgeStateProivder(BaseStateMachine context) : base(context)
    {
        SetState(Minion_States.Alive, new Skulge_AliveState(context, this));
        SetState(Minion_States.Dead, new Skulge_DeadState(context, this));

        SetState(Minion_States.NonCombat, new Skulge_NonCombatState(context, this));
        SetState(Minion_States.Combat, new Skulge_CombatState(context, this));

        SetState(Minion_States.Idle, new Skulge_IdleState(context, this));
        SetState(Minion_States.Chasing, new Skulge_ChasingState(context, this));
    }
}
