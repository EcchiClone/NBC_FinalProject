using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

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
        
    }
    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= interval)
        {
            int key = UnityEngine.Random.Range((int)Pattern.Phase1_Pattern1, (int)Pattern.Phase1_Pattern3 + 1);

            string patternName = Enum.GetName(typeof(Pattern), key);

            Context.Boss.Patterns[(Pattern)key].Invoke(patternName);

            passedTime = 0f;
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

        Vector3 bossPosition = Context.Boss.transform.position;
        Vector3 playerPosition = Context.Boss.Target.transform.position;
        playerPosition.y = bossPosition.y;
        float distance = Vector3.Distance(bossPosition, playerPosition);

        // 공격 대기 중이고 플레이어가 멀리 있는지?
        if (passedTime < interval && distance >= Context.Boss.Data.stopDistance)
        {
            SwitchState(Provider.Chasing());
        }
        // (그로기인지?)
    }

    public override void InitializeSubState()
    {
    }
}
