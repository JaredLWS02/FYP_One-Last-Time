using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Current;

    void Awake()
    {
        if(!Current) Current=this;

        SetupLayers();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Music")]
    public bool musicEnabled=true;

    public GameObject musicLayerPrefab;
    public int numberOfLayers=5;

    List<AudioSource> layers = new List<AudioSource>();
    Dictionary<AudioSource, AudioClip[]> layerClips = new Dictionary<AudioSource, AudioClip[]>();
    
    Dictionary<AudioSource, float> defaultLayerVolumes = new Dictionary<AudioSource, float>();

    public AudioSource currentLayer;
    public int currentLayerIndex;

    public void SetupLayers()
    {
        for(int i=0; i<numberOfLayers; i++)
        {
            AudioSource source = Instantiate(musicLayerPrefab, transform).GetComponent<AudioSource>();
            source.loop=false;

            layers.Add(source);
            layerClips.Add(source, null);

            defaultLayerVolumes.Add(source, source.volume);
            source.volume=0;
        }
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if(musicEnabled)
        {
            AutoReplayAndShuffleAllLayers();
        }
    }

    void AutoReplayAndShuffleAllLayers()
    {
        foreach(AudioSource source in layers)
        {
            if(!source.isPlaying && HasClips(layerClips[source]))
            {
                Play(source);
            }
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public void ChangeMusic(AudioSource source, AudioClip[] clips, float fadeOutTime=3)
    {
        if(DoesLayerHaveSameClips(GetLayerIndex(source), clips)) return;

        if(changingMusicRts.ContainsKey(source))
        {
            if(changingMusicRts[source]!=null) StopCoroutine(changingMusicRts[source]);
            changingMusicRts.Remove(source);
        }

        changingMusicRts[source] = StartCoroutine(ChangingMusic(source, clips, fadeOutTime));
    }
    public void ChangeMusic(int layerIndex, AudioClip[] clips, float fadeOutTime=3)
    {
        ChangeMusic(layers[layerIndex], clips, fadeOutTime);
    }
    
    Dictionary<AudioSource, Coroutine> changingMusicRts = new Dictionary<AudioSource, Coroutine>();

    IEnumerator ChangingMusic(AudioSource source, AudioClip[] clips, float outTime)
    {
        AudioManager.Current.TweenVolume(source, 0, outTime);

        if(outTime>0) yield return new WaitForSecondsRealtime(outTime);

        layerClips[source] = clips;

        if(currentLayer==source) ResetLayerVolume(source);

        Play(source);
    }

    void Play(AudioSource source)
    {
        AudioClip[] clips = layerClips[source];

        if(HasClips(clips))
        {
            source.clip = clips[Random.Range(0, clips.Length)];
        }
        else source.clip = null;

        source.Play();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void ChangeLayer(int layerIndex, float outTime=2, float waitTime=0, float inTime=2)
    {
        if(currentLayer==layers[layerIndex]) return;

        if(crossfadingLayerRts.ContainsKey(layerIndex))
        {
            if(crossfadingLayerRts[layerIndex]!=null) StopCoroutine(crossfadingLayerRts[layerIndex]);
            crossfadingLayerRts.Remove(layerIndex);
        }

        crossfadingLayerRts[layerIndex] = StartCoroutine(CrossfadingLayer(layerIndex, outTime, waitTime, inTime));
    }

    Dictionary<int, Coroutine> crossfadingLayerRts = new Dictionary<int, Coroutine>();

    IEnumerator CrossfadingLayer(int layerIndex, float outTime, float waitTime, float inTime)
    {
        if(currentLayer) AudioManager.Current.TweenVolume(currentLayer, 0, outTime);

        currentLayer = layers[layerIndex];
        currentLayerIndex = layerIndex;

        if(waitTime>0) yield return new WaitForSecondsRealtime(waitTime);

        AudioManager.Current.TweenVolume(currentLayer, defaultLayerVolumes[currentLayer], inTime);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public bool HasClips(AudioClip[] clips)
    {
        return clips!=null && clips.Length>0;
    }
    public bool HasClips(List<AudioClip> clips)
    {
        return HasClips(clips.ToArray());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void ResetLayerVolume(AudioSource layer)
    {
        layer.volume = defaultLayerVolumes[layer];
    }

    public bool DoesLayerHaveSameClips(int layerIndex, AudioClip[] clips)
    {
        AudioClip[] currentClips = layerClips[layers[layerIndex]];

        if(!HasClips(currentClips) || !HasClips(clips)) return false;

        if(currentClips.Length != clips.Length) return false;

        for(int i=0; i<clips.Length; i++)
        {
            if(currentClips[i] != clips[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetLayerIndex(AudioSource source)
    {
        for(int i=0; i<layers.Count; i++)
        {
            if(source==layers[i]) return i;
        }

        Debug.LogWarning("Music Manager: This source was not found in any music layer. Can't get layer index");
        return -1;
    }
}
