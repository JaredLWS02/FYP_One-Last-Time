using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioPrefab : PrefabPreset
{
    [Header("Audio")]
    public AudioSO audioSO;

    // ==================================================================================================================
    
    AudioSource audioSource;

    public AudioSource SpawnAudio()
    {   
        GameObject spawned = Spawn();
        audioSource = spawned.GetComponent<AudioSource>();

        if(!audioSource)
        {
            Debug.LogError("Audio Prefab must have Audio Source!");
            Despawn(spawned);
            return null;
        }

        audioSO.Play(audioSource);

        if(!audioSource.loop)
        Despawn(spawned, audioSource.clip.length);

        return audioSource;
    }

    public void DespawnAudio()
    {
        if(audioSource)
        {
            audioSource.Stop();
            Despawn(audioSource.gameObject);
        }
    }
}
