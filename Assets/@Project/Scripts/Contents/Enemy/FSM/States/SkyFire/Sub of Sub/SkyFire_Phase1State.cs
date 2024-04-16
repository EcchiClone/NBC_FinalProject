using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyFire_Phase1State : BaseState
{
    private float _passedTime;
    private float _attackInterval;

    public SkyFire_Phase1State(BaseStateMachine context, BaseStateProvider provider) 
        : base(context, provider)
    {
        IsRootState = false;
        _attackInterval = Context.Entity.Data.attackInterval;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Phase1");
    }
    public override void UpdateState()
    {
        _passedTime += Time.deltaTime;
        if (_passedTime > _attackInterval)
        {
            int random = UnityEngine.Random.Range(0, 6);

            switch(random)
            {
                case 0:
                    Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 1, true); // 다연발 투사체
                    Context.Entity.enemyPhaseStarter.StartPattern(0, 1, 2, true);
                    break;
                case 1:
                    Context.Entity.enemyPhaseStarter.StartPattern(1, 1, 3, true); // 플라즈마
                    Context.Entity.enemyPhaseStarter.StartPattern(1, 1, 4, true);
                    break;
                case 2:
                    Context.Entity.enemyPhaseStarter.StartPattern(2, 1, 5, true); // 말도안되는1
                    break;
                case 3:
                    Context.Entity.enemyPhaseStarter.StartPattern(3, 1, 5, true); // 말도안되는2
                    break;
                case 4:
                    Context.Entity.enemyPhaseStarter.StartPattern(4, 1, 5, true); // 말도안되는3
                    break;
                case 5:
                    Debug.Log("Pattern Passing");
                    break;
            }

            _passedTime = 0f;

            Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Context.Entity.CurrentHelth <= Context.Entity.Data.maxHealth / 2f)
        {
            SwitchState(Provider.GetState(SkyFire_States.Phase2));
        }

    }

    public override void InitializeSubState()
    {
    }
}
