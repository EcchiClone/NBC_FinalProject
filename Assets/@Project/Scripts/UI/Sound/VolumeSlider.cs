using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        Master,
        Music,
        Ambience,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeSlider.value = AudioManager.Instance.masterVolume;
                break;
            case VolumeType.Music:
                volumeSlider.value = AudioManager.Instance.musicVolume;
                break;
            case VolumeType.Ambience:
                volumeSlider.value = AudioManager.Instance.ambienceVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.Instance.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioManager.Instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.Music:
                AudioManager.Instance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.Ambience:
                AudioManager.Instance.ambienceVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SFXVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}
