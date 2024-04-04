using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_AliveState : BaseState
{
    public SkyFire_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.CurrentHelth <= 0f)
            SwitchState(Provider.GetState(SkyFire_States.Dead));
    }

    public override void ExitState()
    {


    }

    public override void InitializeSubState()
    {
        SetSubState(Provider.GetState(SkyFire_States.Chasing));
    }

}
