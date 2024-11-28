using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // ============================================================================
    
    [Header("MusicManager")]
    public AudioLayerManager layerM;
}
