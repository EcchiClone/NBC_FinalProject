using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_Phase3State : BaseState
{
    public SkyFire_Phase3State(BaseStateMachine context, BaseStateProvider provider)
        : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Phase3");

        // 이전 사용중인 패턴 멈추기
        Context.Entity.enemyPhaseStarter.StopBullet();

        Context.Entity.enemyPhaseStarter.StartPattern(2, 6, 5); // 클러스터

        Context.Entity.enemyPhaseStarter.StartPattern(3, 5, 5); // 스피어
    }

    public override void UpdateState()
    {

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
    }
}
