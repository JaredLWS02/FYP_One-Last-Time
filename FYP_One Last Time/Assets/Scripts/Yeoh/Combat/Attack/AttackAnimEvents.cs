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

    public void AttackWindUp()
    {
        EventM.OnAttackWindUp(owner);
    }

    public void AttackRelease()
    {
        EventM.OnAttackRelease(owner);
    }

    public void AttackRecover()
    {
        EventM.OnAttackRecover(owner);
    }
}
