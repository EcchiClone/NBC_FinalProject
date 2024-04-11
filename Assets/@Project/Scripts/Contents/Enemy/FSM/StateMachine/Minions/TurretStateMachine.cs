using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStateMachine : BaseStateMachine
{
    public TurretStateMachine(Entity entity) : base(entity)
    {
    }

    public override void Initialize()
    {
        //TODO
        Provider = new TurretStateProvider(this);
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }

    public override void Reset()
    {
        CurrentState = Provider.GetState(Turret_States.Alive);
        CurrentState.EnterState();
    }
}
