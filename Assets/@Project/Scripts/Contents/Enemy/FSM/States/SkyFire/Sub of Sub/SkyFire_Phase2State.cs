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
        Debug.Log("Enter Phase2");

        // 이전 사용중인 패턴 멈추기
        Context.Entity.enemyPhaseStarter.StopBullet();

        Context.Entity.enemyPhaseStarter.StartPattern(1, 6, 3); //플라즈마
        Context.Entity.enemyPhaseStarter.StartPattern(1, 6, 4);

        Context.Entity.enemyPhaseStarter.StartPattern(3, 5, 5); // 스피어

        Context.Entity.enemyPhaseStarter.StartPattern(5, 4, 0); // 던지기

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Boss_Phase_2, Context.Entity.transform.position);
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
