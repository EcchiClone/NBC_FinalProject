using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFireStateMachine : BossStateMachine
{
    public SkyFireStateMachine(Boss boss) : base(boss)
    {}

    public override void Initialize()
    {
        Provider = new SkyFireStateProvider(this);
        CurrentState = Provider.Chasing();
    }

}
