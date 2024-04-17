using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_Phase2State : BaseState
{
    private float _passedTime;
    private float _attackInterval;

    public SkyFire_Phase2State(BaseStateMachine context, BaseStateProvider provider)
        : base(context, provider)
    {
        IsRootState = false;
        _attackInterval = Context.Entity.Data.attackInterval;
    }


    public override void EnterState()
    {
        Context.Entity.enemyPhaseStarter.isShooting = false;
        Context.Entity.StartCoroutine(Co_Shoot());
    }

    IEnumerator Co_Shoot()
    {
        yield return Util.GetWaitSeconds(3f);

        Context.Entity.enemyPhaseStarter.isShooting = true;

        Context.Entity.enemyPhaseStarter.StartPattern(1, 6, 3); //플라즈마
        Context.Entity.enemyPhaseStarter.StartPattern(1, 6, 4);

        Context.Entity.enemyPhaseStarter.StartPattern(4, 4, 5); // 서클
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
        if (Context.Entity.CurrentHelth <= Context.Entity.Data.maxHealth * 0.3f)
        {
            SwitchState(Provider.GetState(SkyFire_States.Phase3));
        }
    }

    public override void InitializeSubState()
    {
    }
}
