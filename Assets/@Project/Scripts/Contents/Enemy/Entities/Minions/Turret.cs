using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    protected override void Initialize()
    {
        Target = GameObject.Find("Target").transform;
        //Target = Managers.Module.CurrentModule.transform;
        

        Controller = new TurretUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);
    }

}