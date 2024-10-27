using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAnimEvents : MonoBehaviour
{
    public GameObject owner;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }
    
    // ============================================================================
    
    public void StunRecover()
    {
        EventM.OnStunRecover(owner);
    }
}
