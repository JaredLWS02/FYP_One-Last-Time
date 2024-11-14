using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSpawner : MonoBehaviour
{    
    public List<AudioPrefab> audioPrefabs = new();

    public AudioPrefab GetAudioPrefab(string audio_name)
    {
        if(string.IsNullOrEmpty(audio_name))
        {
            Debug.LogWarning("Audio name is null or empty.");
            return null;
        }
        
        AudioPrefab audioPrefab = audioPrefabs.Find(audioPrefab => audioPrefab.audioSO.Name == audio_name);
        
        if(audioPrefab==null)
        {
            Debug.LogWarning($"AudioSO with name '{audio_name}' not found.");
        }

        return audioPrefab;
    }

    // ============================================================================

    AudioSource PlayAndReturn(string audio_name)
    {
        AudioPrefab audio = GetAudioPrefab(audio_name);
        if(audio==null) return null;
        return audio.SpawnAudio();
    }

    public void Play(string audio_name)
    {
        PlayAndReturn(audio_name);
    }

    public void Stop(string audio_name)
    {
        AudioPrefab audio = GetAudioPrefab(audio_name);
        if(audio==null) return;
        audio.DespawnAudio();
    }

    // ============================================================================

    public void StartLoop(string loop_in_name, string loop_name)
    {
        AudioSource source = PlayAndReturn(loop_in_name);
        if(!source) return;
        // wait for loop in to finish
        StartCoroutine(StartingLoop(loop_name, source.clip.length));
    }

    IEnumerator StartingLoop(string loop_name, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(loop_name);
    }
    
    public void StopLoop(string loop_name, string loop_out_name)
    {
        Stop(loop_name);
        Play(loop_out_name);
    }

    // ============================================================================

    [System.Serializable]
    public struct AudioEvents
    {
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;
    }
    [Space]
    public AudioEvents audioEvents;

    void OnEnable()
    {
        audioEvents.EnableEvent?.Invoke();
    }   
    void OnDisable()
    {
        audioEvents.DisableEvent?.Invoke();
    }   
}
