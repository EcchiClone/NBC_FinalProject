using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;


public class SkyFireStateProvider : BaseStateProvider
{
    public SkyFireStateProvider(BaseStateMachine context)
        : base(context)
    {
        SetState(SkyFire_States.Alive, new SkyFire_AliveState(context, this));
        SetState(SkyFire_States.Dead, new SkyFire_DeadState(context, this));

        SetState(SkyFire_States.NonCombat, new SkyFire_NonCombatState(context, this));
        SetState(SkyFire_States.Combat, new SkyFire_CombatState(context, this));

        SetState(SkyFire_States.Chasing, new SkyFire_ChasingState(context, this));
        SetState(SkyFire_States.Phase1, new SkyFire_Phase1State(context, this));
        SetState(SkyFire_States.Phase2, new SkyFire_Phase2State(context, this));
    }
    
}
