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
        => voices.Find(voice => voice.Name == voice_name);

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
