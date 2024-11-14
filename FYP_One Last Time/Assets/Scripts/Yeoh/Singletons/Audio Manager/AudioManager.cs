using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // ==================================================================================================================

    public AudioMixer mixer;
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    [Header("Defaults")]
    public float masterVolume=1;
    public float musicVolume=1;
    public float sfxVolume=1;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {   
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, masterVolume);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, sfxVolume);

        mixer.SetFloat(MASTER_KEY, Log10(masterVolume));
        mixer.SetFloat(MUSIC_KEY, Log10(musicVolume));
        mixer.SetFloat(SFX_KEY, Log10(sfxVolume));
    }

    float Log10(float value)
    {
        return Mathf.Log10(value)*20;
    }

    // ==================================================================================================================

    Dictionary<AudioSource, Tween> volumeTweens = new();
    
    public void TweenVolume(AudioSource source, float to, float time=3)
    {
        if(volumeTweens.ContainsKey(source))
        {
            volumeTweens[source].Stop();
        }
        
        if(time>0) volumeTweens[source] = Tween.AudioVolume(source, to, time, Ease.InOutSine, useUnscaledTime: true);
        else source.volume = to;
    }    
}
