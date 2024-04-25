public class WarMachineStateProvider : BaseStateProvider
{
    public WarMachineStateProvider(BaseStateMachine context) : base(context)
    {
        SetState(Minion_States.Alive, new WarMachine_AliveState(context, this));
        SetState(Minion_States.Dead, new WarMachine_DeadState(context, this));

        SetState(Minion_States.NonCombat, new WarMachine_NonCombatState(context, this));
        SetState(Minion_States.Combat, new WarMachine_CombatState(context, this));

        SetState(Minion_States.Idle, new WarMachine_IdleState(context, this));
        SetState(Minion_States.Chasing, new WarMachine_ChasingState(context, this));
    }
}
