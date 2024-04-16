public class DroneStateProivder : BaseStateProvider
{
    public DroneStateProivder(BaseStateMachine context) : base(context)
    {
        SetState(Minion_States.Alive, new Drone_AliveState(context, this));
        SetState(Minion_States.Dead, new Drone_DeadState(context, this));

        SetState(Minion_States.NonCombat, new Drone_NonCombatState(context, this));
        SetState(Minion_States.Combat, new Drone_CombatState(context, this));

        SetState(Minion_States.Idle, new Drone_IdleState(context, this));
        SetState(Minion_States.Chasing, new Drone_ChasingState(context, this));
    }
}
