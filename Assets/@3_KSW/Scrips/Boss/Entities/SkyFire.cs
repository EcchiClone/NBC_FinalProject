using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkyFire : Boss
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new AirBossController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);

        // 패턴 메서드 넣는 곳
        Patterns.Add(Pattern.Phase1_Pattern1, DummyFunction);
        Patterns.Add(Pattern.Phase1_Pattern2, DummyFunction);
        Patterns.Add(Pattern.Phase1_Pattern3, DummyFunction);

        Patterns.Add(Pattern.Phase2_Pattern1, DummyFunction);
        Patterns.Add(Pattern.Phase2_Pattern2, DummyFunction);
        Patterns.Add(Pattern.Phase2_Pattern3, DummyFunction);
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

    public void DummyFunction(string something)
    {
        Debug.Log(something);
    }
}
