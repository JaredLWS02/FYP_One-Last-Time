using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientController : AudioLayerController
{
    AmbientManager AmbM;

    protected override void OnBaseEnable()
    {
        AmbM = AmbientManager.Current;
        layerM = AmbM.layerM;
    }
}
