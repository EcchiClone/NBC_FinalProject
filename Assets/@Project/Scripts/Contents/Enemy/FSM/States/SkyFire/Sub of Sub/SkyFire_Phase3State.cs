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
        Context.Entity.enemyPhaseStarter.isShooting = false;
        Context.Entity.StartCoroutine(Co_Shoot());
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Boss_Phase_3, Context.Entity.transform.position);
    }

    IEnumerator Co_Shoot()
    {
        yield return Util.GetWaitSeconds(3f);

        Context.Entity.enemyPhaseStarter.isShooting = true;

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
