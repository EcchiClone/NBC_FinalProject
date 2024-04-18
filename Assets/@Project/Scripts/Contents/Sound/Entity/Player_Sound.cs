using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound : MonoBehaviour
{
    private EventInstance _playerFootsteps;
    private EventInstance _playerHovering;
    public bool IsWalking { get; set; }
    public bool IsHovering { get; set; }

    private void Start()
    {
        _playerFootsteps = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_Footsteps);
        _playerHovering = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_BoosterLoop);
    }

    private void FixedUpdate()
    {
        UpdateInstance(_playerFootsteps, IsWalking);
        UpdateInstance(_playerHovering, IsHovering, STOP_MODE.IMMEDIATE);
    }

    private void UpdateInstance(EventInstance instance, bool state, STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
    {
        if (state)
        {
            PLAYBACK_STATE playbackState;
            instance.getPlaybackState(out playbackState);

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                instance.start();
            }
        }
        else
        {
            instance.stop(stopMode);
        }
    }
}
