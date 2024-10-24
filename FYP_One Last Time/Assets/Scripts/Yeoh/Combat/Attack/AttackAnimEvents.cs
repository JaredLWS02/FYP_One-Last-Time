using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimEvents : MonoBehaviour
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
        EventM.OnAttackWindUp(owner);
    }

    public void Release()
    {
        EventM.OnAttackRelease(owner);
    }

    public void Recover()
    {
        EventM.OnAttackRecover(owner);
    }
}
