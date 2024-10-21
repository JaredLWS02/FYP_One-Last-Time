using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HurtScript))]

public class GenericHurt : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Current.TryHurtEvent += OnTryHurt;
    }
    void OnDisable()
    {
        EventManager.Current.TryHurtEvent -= OnTryHurt;
    }

    // ============================================================================
    
    public void OnTryHurt(GameObject attacker, GameObject victim, HurtboxSO attack, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        // check block/parry first before hurting

        EventManager.Current.OnHurt(attacker, victim, attack, contactPoint);
    }
}
