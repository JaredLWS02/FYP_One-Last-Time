using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioLayer
{
    public string layerName = "Silent";
    public AudioSO audioSO;

    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public float defaultVolume=1;

    public void Play()
    {
        audioSO.Play(source); // this will reset volume also
        defaultVolume = source.volume;
        source.loop = false; // to detect when done
    }

    public void Mute() => source.volume=0;

    public void ResetVolume() => source.volume = defaultVolume;

    public bool HasSameSO(AudioSO check_SO) => audioSO==check_SO;

    [HideInInspector]
    public Coroutine coroutine;
}

// ============================================================================

public class AudioLayerManager : MonoBehaviour
{
    AudioManager AudioM;

    void OnEnable()
    {
        AudioM = AudioManager.Current;
    }

    // ============================================================================

    public List<AudioLayer> audioLayers = new();

    void Reset() => audioLayers = new() { new AudioLayer() };

    // ============================================================================

    [Space]
    public GameObject audioLayerPrefab;

    void Awake() => SetupLayers();

    void SetupLayers()
    {
        for(int i=0; i<audioLayers.Count; i++)
        {
            AudioSource source = Instantiate(audioLayerPrefab, transform).GetComponent<AudioSource>();
            if(!source) Debug.LogError($"Audio Layer Prefab has no AudioSource!");

            audioLayers[i].source = source;
            //audioLayers[i].Play();
            audioLayers[i].Mute(); // mute first, unmute when needed
        }

        ValidateLayers();
    }

    void ValidateLayers()
    {
        foreach(var audioLayer in audioLayers)
        {
            if(audioLayer.audioSO == null)
                Debug.LogWarning($"Audio layer '{audioLayer.layerName}' is missing an AudioSO.");

            if(audioLayer.source == null)
                Debug.LogWarning($"Audio layer '{audioLayer.layerName}' has no AudioSource assigned.");
        }
    }
        
    // ============================================================================

    public bool autoReplayShuffle=true;

    void Update()
    {
        if(autoReplayShuffle)
        {
            AutoReplayShuffleAllLayers();
        }
    }

    void AutoReplayShuffleAllLayers()
    {
        foreach(var audioLayer in audioLayers)
        {
            AudioSO so = audioLayer.audioSO;
            if(!so) continue;
            if(!so.HasClips()) continue;

            AudioSource source = audioLayer.source;

            if(!source.isPlaying && currentLayer==audioLayer)
            {
                audioLayer.Play();
            }
        }
    }

    // ============================================================================

    [Header("Debug")]
    public AudioLayer currentLayer;

    void SetCurrentLayer(string layer_name) => currentLayer = GetLayerByName(layer_name);

    public AudioLayer GetLayerByName(string layer_name)
    {
        if(string.IsNullOrEmpty(layer_name))
        {
            Debug.LogWarning("Audio layer name is null or empty.");
            return null;
        }
        
        AudioLayer layer = audioLayers.Find(item => item.layerName == layer_name);
        
        if(layer==null) Debug.LogWarning($"Audio layer with name '{layer_name}' not found.");
        return layer;
    }

    // ============================================================================

    public void ChangeCurrentSO(AudioSO newSO, float fadeOutTime=3) => ChangeSO(currentLayer, newSO, fadeOutTime);

    public void ChangeSO(AudioLayer audioLayer, AudioSO newSO, float fadeOutTime=3)
    {
        if(audioLayer.HasSameSO(newSO)) return;

        if(audioLayer.coroutine!=null) StopCoroutine(audioLayer.coroutine);
        audioLayer.coroutine = StartCoroutine(ChangingSO(audioLayer, newSO, fadeOutTime));
    }

    IEnumerator ChangingSO(AudioLayer audioLayer, AudioSO newSO, float outTime)
    {
        AudioM.TweenVolume(audioLayer.source, 0, outTime);

        yield return new WaitForSecondsRealtime(outTime);

        audioLayer.audioSO = newSO;

        if(currentLayer==audioLayer)
        {
            audioLayer.Play();
        }
    }

    // ============================================================================
    
    public void CrossfadeToLayer(string layer_name, float outTime=3, float waitTime=1, float inTime=3)
    {
        if(currentLayer.layerName==layer_name) return;

        if(currentLayer.coroutine!=null) StopCoroutine(currentLayer.coroutine);
        currentLayer.coroutine = StartCoroutine(CrossfadingLayer(layer_name, outTime, waitTime, inTime));
    }

    IEnumerator CrossfadingLayer(string layer_name, float outTime, float waitTime, float inTime)
    {
        if(currentLayer.source)
        AudioM.TweenVolume(currentLayer.source, 0, outTime);

        SetCurrentLayer(layer_name);

        yield return new WaitForSecondsRealtime(waitTime);

        AudioM.TweenVolume(currentLayer.source, currentLayer.defaultVolume, inTime);
    }
}
