using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMState
{
    STOP,
    MAINSCENE,
    PERKSCENE,
    TUTORIAL,
    FIELD,
    BOSS,
}

public class BGMPlayer : MonoBehaviour
{
    private Scene _scene;
    private BGMState _state;

    private EventInstance _mainAmbience;
    private EventInstance _perkAmbience;
    private EventInstance _fieldBGM;

    private void Awake()
    {
        _scene = SceneManager.GetActiveScene();
    }

    private void Start()
    {
        GetEventInstances();
    }

    private void FixedUpdate()
    {
        UpdateState();
        UpdateSound();
    }

    private void GetEventInstances()
    {
        // BGM & Ambience 가져오기
        _mainAmbience = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Main_Ambience);
        _perkAmbience = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Perk_Ambience);
        _fieldBGM = AudioManager.Instance.CreateInstace(FMODEvents.Instance.Field_BGM);

        // Parameter 초기화
        _fieldBGM.setParameterByName("FieldBGMState", 0f); // 0f ~ 0.50f: 필드BGM, 0.51f ~ 1.0f: 보스BGM
        _fieldBGM.setParameterByName("FieldLowPass", 0f); // 0f: 필터 효과 없음, 1f: 필터 효과 최대
    }

    private void UpdateState()
    {
        _scene = SceneManager.GetActiveScene();

        if (_scene.name == "MainScene")
        {
            _state = BGMState.MAINSCENE;
        }
        else if (_scene.name == "PerkViewerScene")
        {
            _state = BGMState.PERKSCENE;
        }
        else if (_scene.name == "TutorialScene")
        {
            _state = BGMState.TUTORIAL;
        }
        else if ( _scene.name == "DevScene")
        {
            _state = BGMState.FIELD;
        }
        else
        {
            _state = BGMState.STOP;
        }
    }

    private void UpdateSound()
    {
        switch (_state)
        {
            case BGMState.MAINSCENE:
                PlayInstance(_mainAmbience); break;
            case BGMState.PERKSCENE:
                PlayInstance(_perkAmbience); break;
            case BGMState.TUTORIAL:
                // PlayInstance(_mainAmbience);
                break;
            case BGMState.FIELD:
                PlayInstance(_fieldBGM); break;
            case BGMState.BOSS:
                break;
            default:
                StopInstances(); break;
        }
    }

    private void StopInstances()
    {
        _mainAmbience.stop(STOP_MODE.ALLOWFADEOUT);
        _perkAmbience.stop(STOP_MODE.ALLOWFADEOUT);
        _fieldBGM.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void PlayInstance(EventInstance eventInstance)
    {
        PLAYBACK_STATE playbackState;
        eventInstance.getPlaybackState(out playbackState);

        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            eventInstance.start();
        }
    }
}
