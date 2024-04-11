using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStateProvider : BaseStateProvider
{
    public TurretStateProvider(BaseStateMachine context) : base(context)
    {
        SetState(Turret_States.Alive, new Turret_AliveState(context, this));
        SetState(Turret_States.Dead, new Turret_DeadState(context, this));

        SetState(Turret_States.Attack, new Turret_AttackState(context, this));
    }
}
