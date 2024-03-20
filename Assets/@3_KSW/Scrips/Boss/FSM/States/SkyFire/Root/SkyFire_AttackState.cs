using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_AttackState : BossBaseState
{
    public SkyFire_AttackState(BossStateMachine context, BossStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
    public override void CheckSwitchStates()
    {
        // 공격 시간이 완료 됐는지?
        // 죽었는지?
        // (그로기인지?)
    }

    public override void InitializeSubState()
    {
    }
}
