using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicLayer
{
    public string layerName="Default";
    public AudioClip[] clips;
}

// ============================================================================

public class SceneMusic : MonoBehaviour
{
    public static SceneMusic Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // ============================================================================

    MusicManager MusicM;

    void OnEnable()
    {
        MusicM = MusicManager.Current;
    }

    // ============================================================================

    public List<MusicLayer> musicLayers = new();

    void Reset() => musicLayers = new() { new MusicLayer() };

    // ============================================================================

    int currentLayerIndex;

    public void GetLayerIndex(string layer_name)
    {
        if(string.IsNullOrEmpty(layer_name))
        {
            Debug.LogWarning("Layer name is null or empty.");
            currentLayerIndex = -1;
            return;
        }
        
        currentLayerIndex = musicLayers.FindIndex(item => item.layerName == layer_name);
        
        if(currentLayerIndex==-1) Debug.LogWarning($"Layer with name '{layer_name}' not found.");
    }

    // ============================================================================

    public float startFadeIn=1;

    void Start()
    {
        for(int i=0; i<musicLayers.Count && i<MusicM.numberOfLayers; i++)
        {
            //if(!MusicManager.Current.DoesLayerHaveSameClips(i, musicLayers[i].clips))
            MusicM.ChangeMusic(i, musicLayers[i].clips, startFadeIn);
        }

        ChangeToDefaultLayer();
    }

    public int defaultLayerIndex=0;

    public void SetDefaultLayer(int to) => defaultLayerIndex = to;

    public void SetDefaultLayer(string layer_name)
    {
        GetLayerIndex(layer_name);
        SetDefaultLayer(currentLayerIndex);
    }

    public void ChangeToDefaultLayer() => MusicM.ChangeLayer(defaultLayerIndex);

    // ============================================================================

    public void ChangeToLayerName(string layer_name)
    {
        GetLayerIndex(layer_name);
        MusicM.ChangeLayer(currentLayerIndex);
    }
}
