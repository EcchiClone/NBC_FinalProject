using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_CombatState : BaseState
{
    public Ball_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }
    public override void EnterState()
    {
        Context.Entity.GetDamaged(99999);
    }

    public override void UpdateState() 
    {
        Debug.Log("으아아 공격");
        // 탄막 쪼라라라락
    }

    public override void ExitState(){}

    public override void CheckSwitchStates() { }

    public override void InitializeSubState(){}  
}
