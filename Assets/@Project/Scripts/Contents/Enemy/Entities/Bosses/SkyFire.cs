using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyFire : Entity
{
    public Transform launcher1;
    public Transform launcher2;
    public Transform launcher3;

    

    protected override void Initialize()
    {
        //Target = FindObjectOfType<TargetCenter>().transform;
        CurrentHelth = Data.maxHealth;

        Controller = new AirUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);


        Controller.SetDestination(Target.position);
    }

}
