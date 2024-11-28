using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : AudioLayerController
{
    MusicManager MusicM;

    void OnEnable()
    {
        MusicM = MusicManager.Current;
        layerM = MusicM.layerM;
    }
}
