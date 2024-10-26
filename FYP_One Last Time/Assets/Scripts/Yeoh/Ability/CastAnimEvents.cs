using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastAnimEvents : MonoBehaviour
{
    public GameObject owner;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }
    
    // ============================================================================

    public void WindUp()
    {
        EventM.OnCastWindUp(owner);
    }

    public void Release()
    {
        EventM.OnCastRelease(owner);
    }

    public void Recover()
    {
        EventM.OnCastRecover(owner);
    }
}
