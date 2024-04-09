using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_AliveState : BaseState
{
    public Spider_AliveState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _currentSubState?.EnterState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        // 코루틴 쓰고 있으면 모든 코루틴 멈추는 코드 넣어야함.
        if (!Context.Entity.IsAlive)
            SwitchState(Provider.GetState(Spider_States.Dead));
    }

    public override void ExitState()
    {
        // 애니메이션 등 뭔가 다 멈춰버리기 or 네비매쉬 강체 해제?
    }

    public override void InitializeSubState() // 처음 적용할 상태
    {
        SetSubState(Provider.GetState(Spider_States.NonCombat));
    }

}
