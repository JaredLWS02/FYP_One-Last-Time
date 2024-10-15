using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider, musicSlider, sfxSlider;

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Start() // if player prefs key not found, use the second param as default
    {
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, AudioManager.Current.masterVolume);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, AudioManager.Current.musicVolume);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, AudioManager.Current.sfxVolume);
    }

    void SetMasterVolume(float value)
    {
        mixer.SetFloat(AudioManager.MASTER_KEY, Log10(value));
    }
    
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(AudioManager.MUSIC_KEY, Log10(value));
    }
    
    void SetSFXVolume(float value)
    {
        mixer.SetFloat(AudioManager.SFX_KEY, Log10(value));
    }

    float Log10(float value)
    {
        return Mathf.Log10(value)*20;
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

}
