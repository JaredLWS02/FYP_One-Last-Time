using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HurtScript))]

public class GenericHurt : MonoBehaviour
{
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
        if(victim!=gameObject) return;

        // check block/parry first before hurting

        EventM.OnHurt(victim, attacker, hurtbox, contactPoint);
    }
}
