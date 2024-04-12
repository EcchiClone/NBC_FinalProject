using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new GroundUnitController(this);
        Controller.Initialize();

        StateMachine = new SpiderStateMachine(this);
    }
}
