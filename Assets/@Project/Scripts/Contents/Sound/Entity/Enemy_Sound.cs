using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(StudioEventEmitter))]
public class Enemy_Sound : MonoBehaviour
{
    [SerializeField] private EnemySoundType _soundType;
    private StudioEventEmitter _emitter;

    private void Start()
    {
        GetEventEmitter();
    }

    private void GetEventEmitter()
    {
        switch (_soundType)
        {
            case EnemySoundType.SPIDER:
                _emitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.Instance.Spider_Footsteps, this.gameObject);
                break;
        }
    }

    public void StartEmitter()
    {
        _emitter.Play();
    }

    public void StopEmitter()
    {
        _emitter.Stop();
    }
}
