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

    public AudioSource PlayAudio(AudioPrefab audio) => audio?.SpawnAudio();
    public void StopAudio(AudioPrefab audio) => audio?.DespawnAudio();

    // ============================================================================

    public void Play(string audio_name)
    {
        PlayAndReturn(audio_name);
    }

    AudioSource PlayAndReturn(string audio_name)
    {
        AudioPrefab audio = GetAudioPrefab(audio_name);
        return PlayAudio(audio);
    }

    public void Stop(string audio_name)
    {
        AudioPrefab audio = GetAudioPrefab(audio_name);
        StopAudio(audio);
    }

    // ============================================================================

    [System.Serializable]
    public class AudioLoopGroup
    {
        public string groupName;
        [Space]
        public string loopInName;
        public string loopName;
        public string loopOutName;
    }

    public List<AudioLoopGroup> loopGroups = new();

    public AudioLoopGroup GetLoopGroup(string group_name)
    {
        if(string.IsNullOrEmpty(group_name))
        {
            Debug.LogWarning("Group name is null or empty.");
            return null;
        }
        
        AudioLoopGroup loopGroup = loopGroups.Find(group => group.groupName == group_name);
        
        if(loopGroup==null)
        {
            Debug.LogWarning($"Audio Group Name '{group_name}' not found.");
        }

        return loopGroup;
    }

    // ============================================================================
    
    public void StartLoopGroup(string group_name)
    {
        AudioLoopGroup loopGroup = GetLoopGroup(group_name);
        if(loopGroup==null) return;

        StartLoop(loopGroup.loopInName, loopGroup.loopName);
    }

    public void StartLoop(string loop_in_name, string loop_name)
    {
        AudioSource source = PlayAndReturn(loop_in_name);
        // wait for loop in to finish, if there is one
        float delay = source ? source.clip.length : 0;

        StartCoroutine(StartingLoop(loop_name, delay));
    }

    IEnumerator StartingLoop(string loop_name, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(loop_name);
    }

    public void StopLoopGroup(string group_name)
    {
        AudioLoopGroup loopGroup = GetLoopGroup(group_name);
        if(loopGroup==null) return;
        
        StopLoop(loopGroup.loopInName, loopGroup.loopName, loopGroup.loopOutName);
    }

    public void StopLoop(string loop_in_name, string loop_name, string loop_out_name)
    {
        Stop(loop_in_name);
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
