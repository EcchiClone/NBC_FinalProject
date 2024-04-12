using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Entity
{
    protected override void Initialize()
    {
        //Target = GameObject.Find("Target").transform;
        Target = Managers.Module.CurrentModule.transform;
        CurrentHelth = Data.maxHealth;

        Controller = new BallUnitController(this);
        Controller.Initialize();
        StateMachine = new BallStateMachine(this);
    }
}
