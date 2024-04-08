using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    protected override void Initialize()
    {
        //Target = FindObjectOfType<TargetCenter>().transform;
        CurrentHelth = Data.maxHealth;

        Controller = new TurretUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);


        Controller.SetDestination(Target.position);
    }
}