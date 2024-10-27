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

    public void CastWindUp()
    {
        EventM.OnCastWindUp(owner);
    }

    public void CastRelease()
    {
        EventM.OnCastRelease(owner);
    }

    public void CastRecover()
    {
        EventM.OnCastRecover(owner);
    }
}
