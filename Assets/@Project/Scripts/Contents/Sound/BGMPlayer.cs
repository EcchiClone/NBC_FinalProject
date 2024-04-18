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
    FIELD
}

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }

    private Scene _scene;
    private BGMState _state;

    private EventInstance _mainAmbience;
    private EventInstance _perkAmbience;
    private EventInstance _fieldBGM;

    private bool _isLerp;
    private float _lerpStart;
    private float _lerpEnd;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        _scene = SceneManager.GetActiveScene();

        _isLerp = false;

    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnLoadScene;        
        GetEventInstances();
    }

    private void Update()
    {
        UpdateLowPassLerp();
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

        Managers.StageActionManager.OnBossKilled += () => SetFieldBGMState(0f);

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
        else if (_scene.name == "Tutorial")
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
            default:
                StopInstances(); break;
        }
    }

    public void SetFieldBGMState(float value) 
    {
        _fieldBGM.setParameterByName("FieldBGMState", value);
    }

    public void SetFieldLowPass(float value)
    {
        _fieldBGM.setParameterByName("FieldLowPass", value);
    }

    private void UpdateLowPassLerp()
    {
        if (_isLerp)
        {
            _lerpStart = Mathf.Lerp(_lerpStart, _lerpEnd, 0.5f);
            SetFieldLowPass(_lerpStart);
        }
    }    

    public void SetLowPassLerpVars(float start, float end, float time)
    {        
        _isLerp = true;
        _lerpStart = start;
        _lerpEnd = end;
        Instance.Invoke("StopLerp", time);
    }

    private void StopLerp()
    {
        _isLerp = false;
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

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        Instance = this;
        StopInstances();
        GetEventInstances();
    }
}
