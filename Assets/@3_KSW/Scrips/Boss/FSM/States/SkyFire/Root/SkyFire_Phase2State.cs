using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFire_Phase2State : BossBaseState
{
    private float interval = 2.0f;
    private float passedTime;

    public SkyFire_Phase2State(BossStateMachine context, BossStateProvider provider)
        : base(context, provider)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Phase2");
    }
    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= interval)
        {
            int random = UnityEngine.Random.Range(0, 6);


            switch (random)
            {
                case 0:
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 1, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 2, true);

                    Context.Boss.enemyPhaseStarter.StartPhase(1, 3, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(1, 4, true);
                    Debug.Log("Pattern 2-1");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴1");
                    break;
                case 1:
                    Context.Boss.enemyPhaseStarter.StartPhase(1, 3, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(1, 4, true);

                    Context.Boss.enemyPhaseStarter.StartPhase(2, 5, true);
                    Debug.Log("Pattern 2-2");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴2");
                    break;
                case 2:
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 1, true);
                    Context.Boss.enemyPhaseStarter.StartPhase(0, 2, true);

                    Context.Boss.enemyPhaseStarter.StartPhase(2, 5, true);
                    Debug.Log("Pattern 2-3");
                    //Context.Boss.Patterns[(Pattern)random].Invoke("패턴3");
                    break;
                case 3:
                case 4:
                case 5:
                    Debug.Log("Pattern Passing");
                    break;
            }

            passedTime = 0f;

            Context.Boss.Controller.SetDestination(Context.Boss.Target.position);
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        // 죽었는지?
        if (!Context.Boss.IsAlive)
        {
            SwitchState(Provider.Dead());
            return;
        }

        // 공격 대기 중이고 플레이어가 멀리 있는지?
        if (passedTime < interval && Context.Boss.Controller.IsMoving)
        {
            SwitchState(Provider.Chasing());
        }
    }

    public override void InitializeSubState()
    {
    }
}
