using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyFire_Phase1State : BossBaseState
{
    private float interval = 2.0f;
    private float passedTime;

    

    public SkyFire_Phase1State(BossStateMachine context, BossStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Phase1");
    }
    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= interval)
        {
            int random = UnityEngine.Random.Range(0, 6);


            switch(random)
            {
                case 0:
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 1, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 2, true);
                    Debug.Log("Pattern 1-1");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴1");
                    break;
                case 1:
                    Context.Boss.enemyPhaseStarter.StartPhase(1, 3, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(1, 4, true);
                    Debug.Log("Pattern 1-2");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴2");
                    break;
                case 2:
                    Context.Boss.enemyPhaseStarter.StartPhase(2, 5, true);
                    Debug.Log("Pattern 1-3");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴3");
                    break;
                case 3:
                case 4:
                case 5:
                    Debug.Log("Pattern Passing");
                    break;
            }

            passedTime = 0f;

            Context.Boss.Controller.SetDestination(Context.Boss.Target.position);
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        // 죽었는지?
        if(!Context.Boss.IsAlive)
        {
            SwitchState(Provider.Dead());
            return;
        }

        // 공격 대기 중이고 플레이어가 멀리 있는지?
        if (passedTime < interval && Context.Boss.Controller.IsMoving)
        {
            SwitchState(Provider.Chasing());
        }

        // 체력 50퍼센트 이하인지
        if(Context.Boss.CurrentHelth <= Context.Boss.Data.maxHealth / 2f)
        {
            SwitchState(Provider.Phase2());
        }
    }

    public override void InitializeSubState()
    {
    }
}
