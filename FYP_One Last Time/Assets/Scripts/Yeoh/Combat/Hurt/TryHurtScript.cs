using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryHurtScript : MonoBehaviour
{
    public GameObject owner;
    public ParryScript parry;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryHurtEvent += OnTryHurt;
    }
    void OnDisable()
    {
        EventM.TryHurtEvent -= OnTryHurt;
    }

    // ============================================================================
    
    void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        // check block/parry first
        if(IsParryRaised() && IsFacing(contactPoint) && hurtbox.isParryable)
        {
            EventM.OnParrySuccess(owner, attacker, hurtbox, contactPoint);
            return;
        }

        // else hurt
        EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    bool IsParryRaised()
    {
        if(!parry) return false;
        return parry.isParryRaised;
    }

    bool IsFacing(Vector3 target_pos)
    {
        if(!parry) return false;
        return parry.IsFacing(target_pos);
    }

}
