using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : RandomUpdate
{
    public static AmbientManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // ============================================================================
    
    [Header("AmbientManager")]
    public AudioLayerManager layerM;

    // ============================================================================

    public void ToggleAmbShort(bool toggle) => enableSlowUpdate=toggle;
}
