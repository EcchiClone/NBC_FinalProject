using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_ChasingState : BaseState
{
    private float chasingInterval;
    private float passedTime;

    private EventInstance _spiderFootsteps;

    private bool _isWalking = false;

    public Spider_ChasingState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;

        chasingInterval = Context.Entity.Data.chasingInterval;
        _entityTransform = Context.Entity.transform;
        _targetTransform = Context.Entity.Target.transform;

        _spiderFootsteps = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_BoosterLoop);
    }
    public override void EnterState()
    {
        passedTime = 0f;
        Context.Entity.Controller.SetDestination(Context.Entity.Target.position);

        _isWalking = true;
    }
    
    public override void UpdateState()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= chasingInterval)
        {
            Context.Entity.Controller.SetDestination(Context.Entity.Target.position);
            passedTime = 0f;
        }
        UpdateSound();

        CheckSwitchStates();
    }    

    public override void CheckSwitchStates()
    {
        float distance = Vector3.Distance(_entityTransform.position, _targetTransform.position);
        if (Context.Entity.Data.cognizanceRange < distance)
        {
            SwitchState(Context.Provider.GetState(Spider_States.Idle));
        }
    }

    public override void ExitState()
    {
        _isWalking = false;
    }

    public override void InitializeSubState()
    {
    }

    private void UpdateSound()
    {
        if (_isWalking)
        // 걸어가는 상태일 때
        {
            PLAYBACK_STATE playbackState;
            _spiderFootsteps.getPlaybackState(out playbackState);

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _spiderFootsteps.start();
            }
        }
        else
        {
            _spiderFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
