using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAnimEvents : MonoBehaviour
{
    public GameObject owner;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }
    
    // ============================================================================
    
    public void ParryRecover()
    {
        EventM.OnParryRecover(owner);
    }
}
