using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : AudioLayerController
{
    MusicManager MusicM;

    protected override void OnBaseEnable()
    {
        MusicM = MusicManager.Current;
        layerM = MusicM.layerM;
    }
}
