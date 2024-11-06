using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HurtScript))]

public class SimpleTryHurt : MonoBehaviour
{
    public GameObject owner;

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

        // check block/parry first before hurting

        EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
    }
}
