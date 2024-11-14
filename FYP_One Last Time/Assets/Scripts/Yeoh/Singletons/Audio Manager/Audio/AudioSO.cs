using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName="New Audio", menuName="SO/AudioSO")]

public class AudioSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;
    [Space]

    // ============================================================================

    [SerializeField]
    List<AudioClip> randomClips = new();
    public AudioClip GetRandomClip() => randomClips[Random.Range(0,randomClips.Count)];

    public AudioMixerGroup output;

    public bool loop;

    [SerializeField]
    Vector2 randomVolume = Vector2.one;
    public float GetRandomVolume() => Random.Range(randomVolume.x,randomVolume.y);

    [SerializeField]
    Vector2 randomPitch = new(.9f, 1.1f);
    public float GetRandomPitch() => Random.Range(randomPitch.x,randomPitch.y);

    [SerializeField]
    Vector2 randomStereoPan = Vector2.zero;
    public float GetRandomPan() => Random.Range(randomStereoPan.x,randomStereoPan.y);

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
        source.panStereo = GetRandomPan();
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
