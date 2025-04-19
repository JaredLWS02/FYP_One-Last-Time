using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyviActions : EnemyActions
{
    [Header("Spyvi Toggles")]
    public bool AllowParry;
    public bool AllowPhase;

    // ============================================================================
    
    protected override void OnEnable()
    {
        base.OnEnable();

        EventM.TryRaiseParryEvent += OnTryRaiseParry;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        EventM.TryRaiseParryEvent -= OnTryRaiseParry;
    }

    // ============================================================================
    
    void OnTryRaiseParry(GameObject who)
    {
        if(who!=owner) return;
        if(!AllowParry) return;

        EventM.OnRaiseParry(owner);
    }

    // ============================================================================
        
    protected override void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;
        if(!AllowHurt) return;

        EventM.OnTryParry(owner, attacker, hurtbox, contactPoint);
        //EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    // called by OnSlowUpdate Event
    public void CheckPhase()
    {
        if(AllowPhase)
        phase?.TryChangePhase();
    }

    // ============================================================================
    
    [Header("Check Action States")]

    public TryParryScript tryParry;
    public bool IsTryingToParry() => tryParry.IsPerforming();

    public OnParryScript onParry;
    public bool IsParrying() => onParry.IsPerforming();

    public PhaseScript phase;
    public bool IsPhasing() => phase?.IsPerforming() ?? false;
    
}
