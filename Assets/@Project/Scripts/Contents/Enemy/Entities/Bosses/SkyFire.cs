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
        Target = FindObjectOfType<TargetCenter>().transform;
        CurrentHelth = Data.maxHealth;

        Controller = new AirUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);

        

        // 패턴 메서드 넣는 곳
        Patterns.Add(Pattern.Phase1_Pattern1, DummyFunction);
        Patterns.Add(Pattern.Phase1_Pattern2, DummyFunction);
        Patterns.Add(Pattern.Phase1_Pattern3, DummyFunction);

        Patterns.Add(Pattern.Phase2_Pattern1, DummyFunction);
        Patterns.Add(Pattern.Phase2_Pattern2, DummyFunction);
        Patterns.Add(Pattern.Phase2_Pattern3, DummyFunction);

        Controller.SetDestination(Target.position);
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
