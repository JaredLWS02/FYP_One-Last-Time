using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AudioPrefab : PrefabPreset
{
    [Header("Audio")]
    public AudioSO audioSO;
    
    public AudioSource SpawnAudio()
    {   
        GameObject spawned = Spawn();
        AudioSource audioSource = spawned.GetComponent<AudioSource>();

        if(!audioSource)
        {
            Debug.LogError($"AudioPrefab: {prefabName} must have Audio Source!");
            Despawn(spawned);
            return null;
        }

        audioSO.Play(audioSource);

        if(!audioSource.loop)
        Despawn(spawned, audioSource.clip.length);

        return audioSource;
    }
}

// ==================================================================================================================

public class AudioSpawner : MonoBehaviour
{    
    public List<AudioPrefab> audioPrefabs = new();

    void Reset() => audioPrefabs = new() { new AudioPrefab() };

    // ==================================================================================================================

    AudioPrefab currentAudioPrefab;

    public void GetAudioPrefab(string audio_name)
    {
        if(string.IsNullOrEmpty(audio_name))
        {
            Debug.LogWarning("Audio name is null or empty.");
            currentAudioPrefab = null;
            return;
        }
        
        currentAudioPrefab = audioPrefabs.Find(audioPrefab => audioPrefab.prefabName == audio_name);
        
        if(currentAudioPrefab==null) Debug.LogWarning($"AudioSO with name '{audio_name}' not found.");
    }

    // ============================================================================

    public void SetSpawnPos(Vector3 pos) => currentAudioPrefab.spawnPos = pos;

    // ============================================================================

    public AudioSource PlayAudio(AudioPrefab audio) => audio?.SpawnAudio();

    public void Play() => PlayAudio(currentAudioPrefab);

    AudioSource PlayAndReturn(string audio_name)
    {
        GetAudioPrefab(audio_name);
        return PlayAudio(currentAudioPrefab);
    }

    public void PlayName(string audio_name) => PlayAndReturn(audio_name);

    // ============================================================================
    
    public void StopAudio(AudioPrefab audio) => audio?.Despawn();

    public void Stop() => StopAudio(currentAudioPrefab);
    
    public void Stop(string audio_name)
    {
        GetAudioPrefab(audio_name);
        Stop();
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
        PlayName(loop_name);
    }

    // ============================================================================

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
        PlayName(loop_out_name);
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

    void OnEnable() => audioEvents.EnableEvent?.Invoke();
    void OnDisable() => audioEvents.DisableEvent?.Invoke();
}
