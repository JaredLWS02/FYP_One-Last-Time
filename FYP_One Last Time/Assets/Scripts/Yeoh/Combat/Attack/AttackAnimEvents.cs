using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimEvents : MonoBehaviour
{
    public GameObject owner;
    
    // ============================================================================

    public void WindUp()
    {
        EventManager.Current.OnAttackWindUp(owner);
    }

    public void Release()
    {
        EventManager.Current.OnAttackRelease(owner);
    }

    public void Recover()
    {
        EventManager.Current.OnAttackRecover(owner);
    }
}
