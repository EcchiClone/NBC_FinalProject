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

        Context.Entity.enemyPhaseStarter.StartPhase(0, 5); // MG Special 




        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Boss_Phase_3, Context.Entity.transform.position);
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
