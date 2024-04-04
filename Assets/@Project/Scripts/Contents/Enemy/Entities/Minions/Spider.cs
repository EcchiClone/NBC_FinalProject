using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Entity
{
    protected override void Initialize()
    {
        //Target = FindObjectOfType<TargetCenter>().transform;
        CurrentHelth = Data.maxHealth;

        Controller = new GroundUnitController(this);
        Controller.Initialize();

        Controller?.SetDestination(Target.position);
    }
}
