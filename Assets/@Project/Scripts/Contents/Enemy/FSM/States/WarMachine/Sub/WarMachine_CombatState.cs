using UnityEngine;

public class WarMachine_CombatState : BaseState
{
    public WarMachine_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }

    public override void EnterState()
    {
        // 이전 사용중인 패턴 멈추기
        Context.Entity.enemyPhaseStarter.StopBullet();

        Context.Entity.enemyPhaseStarter.StartPattern(0, 3, 1);
        Context.Entity.enemyPhaseStarter.StartPattern(0, 3, 2);
        Context.Entity.enemyPhaseStarter.StartPattern(1, 3, 3);
        Context.Entity.enemyPhaseStarter.StartPattern(1, 3, 4);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // 페이즈 끄기
        Context.Entity.enemyPhaseStarter.StopBullet();
        Context.Entity.enemyPhaseStarter.StopPattern();
    }

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Stat.stopDistance <= distance)
        {
            SwitchState(Context.Provider.GetState(Minion_States.NonCombat));
        }
    }

    public override void InitializeSubState()
    {
    }
}
