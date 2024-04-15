using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStateProvider : BaseStateProvider
{
    public TurretStateProvider(BaseStateMachine context) : base(context)
    {
        SetState(Turret_States.Alive, new Turret_AliveState(context, this));
        SetState(Turret_States.Dead, new Turret_DeadState(context, this));

        SetState(Turret_States.NonCombat, new Turret_NonCombatState(context, this));
        SetState(Turret_States.Combat, new Turret_CombatState(context, this));

        SetState(Turret_States.Idle, new Turret_IdleState(context, this));
    }
}
