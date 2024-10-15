using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithLoudness : MonoBehaviour
{
    MusicManager musicManager;

    void GetMusicManager()
    {
        if(!musicManager) musicManager = MusicManager.Current;
    }

    void Update()
    {
        GetMusicManager();

        if(!musicManager) return;

        UpdateLoudness();

        UpdateScale();
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public float loudnessSensibility=1;
    public float threshold=.1f;

    float loudness;

    void UpdateLoudness()
    {
        loudness = GetLoudness(musicManager.currentLayer.timeSamples, musicManager.currentLayer.clip) * loudnessSensibility;

        if(loudness<threshold) loudness=0;
    }

    public int sampleWindow=2048;

    float GetLoudness(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition-sampleWindow;

        if(startPosition<0) return 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, startPosition);

        float totalLoudness=0;

        for(int i=0;i<sampleWindow;i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness/sampleWindow;
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector3 minScale;
    public Vector3 maxScale;

    void UpdateScale()
    {
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }

    [ContextMenu("Get Current Scale")]
    public void GetCurrentScale()
    {
        minScale = maxScale = transform.localScale;
    }
}
