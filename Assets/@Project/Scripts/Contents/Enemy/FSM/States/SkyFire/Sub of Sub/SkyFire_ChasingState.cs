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
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
        UnityEngine.Debug.Log("보스 추적 입장");
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
        Debug.Log("SkyFire : Exit Chasing State");
    }

    public override void CheckSwitchStates()
    {

        /*if(!Context.Entity.Controller.IsMoving) // 일단 컨트롤러의 IsMoving이 false면 바로 공격으로
        {
            SwitchState(Provider.GetState(SkyFire_States.Phase1));
        }*/
    }

    public override void InitializeSubState()
    {
        // 일단 없음
    }

}
