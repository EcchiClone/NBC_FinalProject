using System.Collections.Generic;

public class BallStateProvider : BaseStateProvider
{
    public BallStateProvider(BaseStateMachine context) : base(context)
    {
        SetState(Minion_States.Alive, new Ball_AliveState(context, this));
        SetState(Minion_States.Dead, new Ball_DeadState(context, this));


        SetState(Minion_States.NonCombat, new Ball_NonCombatState(context, this));
        SetState(Minion_States.Combat, new Ball_CombatState(context, this));


        SetState(Minion_States.Idle, new Ball_IdleState(context, this));
        SetState(Minion_States.Chasing, new Ball_ChasingState(context, this));
    }
}
