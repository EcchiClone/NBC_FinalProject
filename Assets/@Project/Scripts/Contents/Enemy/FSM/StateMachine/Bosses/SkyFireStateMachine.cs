using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFireStateMachine : BaseStateMachine
{
    public SkyFireStateMachine(Entity boss) : base(boss)
    {}

    public override void Initialize()
    {
        Provider = new SkyFireStateProvider(this);
        CurrentState = Provider.Chasing();
    }

}
