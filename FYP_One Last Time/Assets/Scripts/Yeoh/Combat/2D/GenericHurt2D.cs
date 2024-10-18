using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hurt2D))]

public class GenericHurt2D : MonoBehaviour
{
    Hurt2D hurt;

    void Awake()
    {
        hurt = GetComponent<Hurt2D>();
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.Hit2DEvent += OnHit;
    }
    void OnDisable()
    {
        EventManager.Current.Hit2DEvent -= OnHit;
    }

    // ============================================================================
    
    public void OnHit(GameObject attacker, GameObject victim, HurtInfo2D hurtInfo)
    {
        if(victim!=gameObject) return;

        // check block/parry first before hurting

        hurt.Hurt(attacker, hurtInfo);
    }
}
