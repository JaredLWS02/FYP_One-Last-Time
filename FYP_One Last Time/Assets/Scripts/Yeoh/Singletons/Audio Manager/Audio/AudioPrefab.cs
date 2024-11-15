using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioPrefab : PrefabPreset
{
    [Header("Audio")]
    public AudioSO audioSO;

    // ==================================================================================================================
    
    List<AudioSource> audioSources = new();

    public AudioSource SpawnAudio()
    {   
        GameObject spawned = Spawn();
        AudioSource audioSource = spawned.GetComponent<AudioSource>();

        if(!audioSource)
        {
            Debug.LogError("Audio Prefab must have Audio Source!");
            Despawn(spawned);
            return null;
        }

        audioSO.Play(audioSource);

        if(!audioSource.loop)
        Despawn(spawned, audioSource.clip.length);

        audioSources.Add(audioSource);

        return audioSource;
    }

    public void DespawnAudio(AudioSource source)
    {
        if(!audioSources.Contains(source)) return;
        if(!source) return;

        source.Stop();
        Despawn(source.gameObject);
        audioSources.Remove(source);
    }

    public void DespawnAudio()
    {
        // temp duplicate list to iterate through
        // because original list is changing each time 
        List<AudioSource> sources = new(audioSources);

        foreach(var source in sources)
        {
            DespawnAudio(source);
        }
    }
}
