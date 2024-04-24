using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound : MonoBehaviour
{
    private EventInstance _playerFootsteps;
    private EventInstance _playerHovering;
    private EventInstance _playerDash;
    private EventInstance _playerDragging;

    public bool IsWalking { get; set; }
    public bool IsHovering { get; set; }
    public bool IsDashing { get; set; }
    public bool IsDragging { get; set; }

    private void Start()
    {
        _playerFootsteps = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_Footsteps);
        _playerHovering = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_Booster);
        _playerDash = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_Dash);
        _playerDragging = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Player_Dragging);

        Managers.ModuleActionManager.OnChangeArmorPoint += OnChangeArmorPoint;
    }

    private void FixedUpdate()
    {
        UpdateInstance(_playerFootsteps, IsWalking, STOP_MODE.IMMEDIATE);
        UpdateInstance(_playerHovering, IsHovering, STOP_MODE.IMMEDIATE);
        UpdateInstance(_playerDash, IsDashing, STOP_MODE.IMMEDIATE);
        UpdateInstance(_playerDragging, IsDragging, STOP_MODE.IMMEDIATE);
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

    private void StopInstance(EventInstance instance, STOP_MODE stopMode = STOP_MODE.IMMEDIATE)
    {
        instance.stop(stopMode);
    }

    private void OnChangeArmorPoint(float totalAP, float remainAP)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Damaged, Vector3.zero);
    }
}
