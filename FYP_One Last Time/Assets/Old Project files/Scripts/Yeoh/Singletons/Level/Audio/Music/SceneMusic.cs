using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [System.Serializable]
    public class MusicLayer
    {
        public string layerName;
        public AudioClip[] clips;
    }

    public List<MusicLayer> musicLayers = new List<MusicLayer>();

    public int defaultLayer=1;

    void Start()
    {
        for(int i=0; i<musicLayers.Count && i<MusicManager.Current.numberOfLayers; i++)
        {
            // if(!MusicManager.Current.DoesLayerHaveSameClips(i, musicLayers[i].clips))

            MusicManager.Current.ChangeMusic(i, musicLayers[i].clips, 2);
        }

        MusicManager.Current.ChangeLayer(defaultLayer-1);
    }
}
