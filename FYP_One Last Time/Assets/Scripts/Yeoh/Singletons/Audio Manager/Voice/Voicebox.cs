using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Voicebox : RandomUpdate
{
    [Header("Voicebox")]
    public AudioSource voiceSource;

    // ==================================================================================================================

    [System.Serializable]
    public class Voice
    {
        public string voiceName;
        public AudioSO audioSO;
    }

    public List<Voice> voices = new();

    Voice currentVoice;

    public void GetVoice(string voice_name)
    {
        if(string.IsNullOrEmpty(voice_name))
        {
            Debug.LogWarning("Voice name is null or empty.");
            currentVoice = null;
            return;
        }
        
        currentVoice = voices.Find(item => item.voiceName == voice_name);
        
        if(currentVoice==null) Debug.LogWarning($"Voice with name '{voice_name}' not found.");
    }

    // ==================================================================================================================

    public void Play() => currentVoice.audioSO.Play(voiceSource);

    public void Play(string voice_name)
    {
        GetVoice(voice_name);
        Play();
    }

    public void Stop() => voiceSource.Stop();

    // ==================================================================================================================
    
    [System.Serializable]
    public struct VoiceEvents
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
        public UnityEvent OnRandomVoice;
    }
    [Space]
    public VoiceEvents voiceEvents;

    void OnEnable() => voiceEvents.OnEnable?.Invoke();
    void OnDisable() => voiceEvents.OnDisable?.Invoke();
    public override void OnRandomUpdate() => voiceEvents.OnRandomVoice?.Invoke();
}
