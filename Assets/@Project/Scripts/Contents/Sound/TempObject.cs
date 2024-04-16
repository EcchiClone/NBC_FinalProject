using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    // 필드에 선언
    private EventInstance _spiderFootsteps;
    private bool _isWalking; // if문에 들어갈 예시 조건입니다. 조건은 마음대로 수정 가능합니다. 

    private void Start()
    {
        _spiderFootsteps = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Spider_Footsteps);
    }

    private void FixedUpdate()
    {
        UpdateSound();
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
