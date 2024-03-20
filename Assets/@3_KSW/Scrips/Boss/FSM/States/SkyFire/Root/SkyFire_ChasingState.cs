using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_ChasingState : BossBaseState
{
    private float chasingInterval;
    private float passedTime;

    public SkyFire_ChasingState(BossStateMachine context, BossStateProvider provider) 
        : base(context, provider) 
    {
        IsRootState = true;
        chasingInterval = Context.Boss.data.chasingInterval;
    }

    public override void EnterState()
    {
        Debug.Log("SkyFire : Enter Chasing State");
        passedTime = 0f;
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Context.Boss.Controller.SetDestination(Context.Boss.Target.position);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("SkyFire : Exit Chasing State");
    }
    public override void CheckSwitchStates()
    {
        // 공격할 조건 됐는지 판단
        // 추격 중에 시간 지나면 

        // 죽었는지 판단

        if (!Context.Boss.IsAlive)
            SwitchState(Provider.Dead());
    }

    public override void InitializeSubState()
    {
        // 일단 없음
    }

}
