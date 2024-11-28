using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientController : AudioLayerController
{
    AmbientManager AmbM;

    void OnEnable()
    {
        AmbM = AmbientManager.Current;
        layerM = AmbM.layerM;
    }
}
