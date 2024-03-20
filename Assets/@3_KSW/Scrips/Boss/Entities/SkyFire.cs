using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyFire : Boss
{
    protected override void Initialize()
    {
        Target = FindObjectOfType<PlayerStateMachine>().transform;

        CurrentHelth = Data.maxHealth;

        Controller = new AirBossController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);
    }

    //TODO : update 이벤트 만들어서 묶을 필요 있음
    void Update()
    {
        if (Controller != null) 
        {
            Controller.Update();
        }

        if(StateMachine != null)
        {
            StateMachine.Update();
        }    
    }
}
