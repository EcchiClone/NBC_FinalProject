using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyFire_DeadState : BaseState
{
    public SkyFire_DeadState(BaseStateMachine context, BaseStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        // Add rigidbody
        //Context.Boss.Controller.Stop();
        Context.Entity.GetOrAddComponent<Rigidbody>().useGravity = true;
        Context.Entity.GetOrAddComponent<Rigidbody>().isKinematic = false;


        Debug.Log("SkyFire : Enter Dead State");

        Context.Entity.Invoke("Disappear", 3);
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

    private void Disappear()
    {

        Context.Entity.gameObject.SetActive(false);
    }
}
