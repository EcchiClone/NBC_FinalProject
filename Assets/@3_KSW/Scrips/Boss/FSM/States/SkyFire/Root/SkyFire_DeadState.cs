using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyFire_DeadState : BossBaseState
{
    public SkyFire_DeadState(BossStateMachine context, BossStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        // Add rigidbody
        //Context.Boss.Controller.Stop();
        Context.Boss.GetOrAddComponent<Rigidbody>().useGravity = true;
        Context.Boss.GetOrAddComponent<Rigidbody>().isKinematic = false;


        Debug.Log("SkyFire : Enter Dead State");
    }
    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }
}
