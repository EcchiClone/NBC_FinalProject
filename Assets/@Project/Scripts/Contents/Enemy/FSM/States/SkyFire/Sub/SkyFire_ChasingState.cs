using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;

    public SkyFire_ChasingState(BaseStateMachine context, BaseStateProvider provider) 
        : base(context, provider) 
    {
        IsRootState = false;
        chasingInterval = Context.Entity.Data.chasingInterval;
    }

    public override void EnterState()
    {
        Debug.Log("SkyFire : Enter Chasing State");
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
    }

    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
            passedTime = 0f;
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Context.Boss.Controller.Stop();
        Debug.Log("SkyFire : Exit Chasing State");
    }

    public override void CheckSwitchStates()
    {
        // 공격할 조건 됐는지 판단
        // 추격 중에 시간 지나면 

        // 죽었는지 판단

        if (!Context.Entity.IsAlive)
        {
            SwitchState(Provider.GetState(SkyFire_States.Alive));
            return;
        }

        if(!Context.Entity.Controller.IsMoving) // 일단 컨트롤러의 IsMoving이 false면 바로 공격으로
        {
            SwitchState(Provider.GetState(SkyFire_States.Phase1));
        }
    }

    public override void InitializeSubState()
    {
        // 일단 없음
    }

}
