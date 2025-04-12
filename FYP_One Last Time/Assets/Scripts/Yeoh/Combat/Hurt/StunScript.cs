using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunScript : BaseAction
{
    [Header("Stun script")]
    public bool allowStun=true;
    public AnimSO defaultStunAnim;

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.StunEvent += OnStun;
        EventM.StunAnimEvent += OnStunAnim;
        EventM.CancelStunEvent += OnCancelStun;
    }
    void OnDisable()
    {
        EventM.StunEvent -= OnStun;
        EventM.StunAnimEvent += OnStunAnim;
        EventM.CancelStunEvent -= OnCancelStun;
    }

    // ============================================================================
    
    void OnStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim != owner) return;
        if(!hurtbox.canStun) return;
        if(IsCooling() && !hurtbox.ignoreStunCooldown) return;

        EventM.OnStunAnim(owner, attacker, hurtbox.customStunAnim, contactPoint);
        
        EventM.OnStunned(owner, attacker, hurtbox, contactPoint);
    }

    void OnStunAnim(GameObject victim, GameObject attacker, AnimSO customStunAnim, Vector3 contactPoint)
    {
        if(victim != owner) return;

        DoCooldown();

        // action cancelling
        EventM.OnCancelAutoJump(owner);
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);

        //actionCounter++;

        AnimSO stunAnim = customStunAnim ? customStunAnim : defaultStunAnim;
        Perform(stunAnim);
    }

    // ============================================================================
    
    [Header("After Recover")]
    public Timer cooldown;
    public float cooldownTime=.5f;

    void Update()
    {
        cooldown.canTick = !IsPerforming();
    }

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================

    void OnCancelStun(GameObject who)
    {
        if(who!=owner) return;
        
        if(!IsPerforming()) return;

        CancelAnim();
    }
}
