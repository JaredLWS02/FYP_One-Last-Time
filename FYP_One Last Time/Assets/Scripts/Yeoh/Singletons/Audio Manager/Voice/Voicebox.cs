using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Voicebox : MonoBehaviour
{
    public AudioSource voiceSource;

    // ==================================================================================================================

    public List<AudioSO> voices = new();

    public AudioSO GetVoice(string voice_name)
    {
        if(string.IsNullOrEmpty(voice_name))
        {
            Debug.LogWarning("Voice name is null or empty.");
            return null;
        }
        
        AudioSO audioSO = voices.Find(voice => voice.Name == voice_name);
        
        if(audioSO==null)
        {
            Debug.LogWarning($"Voice AudioSO with name '{voice_name}' not found.");
        }

        return audioSO;
    }

    // ==================================================================================================================

    public void Play(string voice_name)
    {
        AudioSO voice = GetVoice(voice_name);
        voice.Play(voiceSource);
    }

    public void Stop() => voiceSource.Stop();

    // ==================================================================================================================

    [System.Serializable]
    public struct VoiceEvents
    {
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;
    }
    [Space]
    public VoiceEvents voiceEvents;

    void OnEnable()
    {
        voiceEvents.EnableEvent?.Invoke();
    }   
    void OnDisable()
    {
        voiceEvents.DisableEvent?.Invoke();
    }   
}
