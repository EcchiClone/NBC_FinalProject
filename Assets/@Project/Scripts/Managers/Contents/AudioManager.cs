using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private string _path;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float ambienceVolume = 1f;
    [Range(0f, 1f)]
    public float SFXVolume = 1f;


    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;


    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        _path = Path.Combine(Application.dataPath, "VolumeData.txt");

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        LoadVolumeData();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }

    private void OnApplicationQuit()
    {
        SaveVolumeData();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldpos)
    {
        RuntimeManager.PlayOneShot(sound, worldpos);
    }

    public void SetEventParameters(string parameterName, float value)
    {

    }

    public EventInstance CreateInstace(EventReference eventReference)
    {
        EventInstance eventInstace = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstace);
        return eventInstace;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {

    }

    private void OnDestroy()
    {
        CleanUp();
    }

    private void SaveVolumeData()
    {
        float volume;
        string volumeData = "";
        
        masterBus.getVolume(out volume);
        volumeData += volume.ToString("N2");
        volumeData += "-";

        musicBus.getVolume(out volume);
        volumeData += volume.ToString("N2");
        volumeData += "-";

        ambienceBus.getVolume(out volume);
        volumeData += volume.ToString("N2");
        volumeData += "-";

        sfxBus.getVolume(out volume);
        volumeData += volume.ToString("N2");

        File.WriteAllText(_path, volumeData);
    }

    private void LoadVolumeData()
    {
        if (!File.Exists(_path))
        {
            Debug.Log("볼륨 파일 없음!");
            return;
        }
        else
        {
            string volumeData = File.ReadAllText(_path);
            string[] data = volumeData.Split("-");

            masterVolume = float.Parse(data[0]);
            musicVolume = float.Parse(data[1]);
            ambienceVolume = float.Parse(data[2]);
            SFXVolume = float.Parse(data[3]);
        }
    }
}