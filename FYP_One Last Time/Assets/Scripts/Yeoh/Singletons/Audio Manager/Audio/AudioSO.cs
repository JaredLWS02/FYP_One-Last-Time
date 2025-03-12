using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName="New Audio", menuName="SO/AudioSO")]

public class AudioSO : ScriptableObject
{
    [SerializeField]
    List<AudioClip> randomClips = new();
    public AudioClip GetRandomClip() => HasClips() ? randomClips[Random.Range(0,randomClips.Count)] : null;
    public bool HasClips() => randomClips.Count>0;

    public AudioMixerGroup output;

    public bool loop;

    [SerializeField]
    Vector2 randomVolume = Vector2.one;
    public float GetRandomVolume() => Random.Range(randomVolume.x,randomVolume.y);

    [SerializeField]
    Vector2 randomPitch = new(.9f, 1.1f);
    public float GetRandomPitch() => Random.Range(randomPitch.x,randomPitch.y);

    [System.Serializable]
    public struct StereoPan
    {
        [SerializeField]
        [Range(-1,1)]
        float min;
        
        [SerializeField]
        [Range(-1,1)]
        float max;

        public float GetRandom() => Random.Range(min,max);
    };

    public StereoPan stereoPan;

    [Header("3D")]
    [Range(0,1)]
    public float spatialBlend=1;
    public float minDistance=9;
    public float maxDistance=18;

    // ============================================================================
    
    void Setup(AudioSource source)
    {
        source.clip = GetRandomClip();
        source.outputAudioMixerGroup = output;
        source.loop = loop;
        source.volume = GetRandomVolume();
        source.pitch = GetRandomPitch();
        source.panStereo = stereoPan.GetRandom();
        // 3D
        source.spatialBlend = spatialBlend;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
    }

    public void Play(AudioSource source)
    {
        Setup(source);
        source.Play();
    }
    
    // ============================================================================

    [HideInInspector]
    public int ID => GetInstanceID();
}
