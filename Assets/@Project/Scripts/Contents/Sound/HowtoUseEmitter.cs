using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (StudioEventEmitter))]
public class HowtoUseEmitter : MonoBehaviour
{
    private StudioEventEmitter _emitter;

    private void Start()
    {
        _emitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.Instance.Spider_Footsteps, this.gameObject);
    }

    private void StartEmitter() => _emitter.Play();

    private void StopEmitter() => _emitter.Stop();
}
