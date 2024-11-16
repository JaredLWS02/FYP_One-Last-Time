using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunScript : BaseAction
{
    [Header("Stun script")]
    public bool allowStun=true;
    public AnimSO defaultStunAnim;
    AnimSO stunAnim;

    void Awake()
    {
        stunAnim = defaultStunAnim;
    }

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.StunEvent += OnStun;
        EventM.CancelStunEvent += OnCancelStun;
    }
    void OnDisable()
    {
        EventM.StunEvent -= OnStun;
        EventM.CancelStunEvent -= OnCancelStun;
    }

    // ============================================================================
    
    void OnStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;
        if(!hurtbox.canStun) return;
        if(IsCooling()) return;

        // action cancelling
        EventM.OnCancelAutoJump(owner);
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);

        actionCounter++;

        stunAnim = hurtbox.customStunAnim ?
            hurtbox.customStunAnim : defaultStunAnim;

        Perform(stunAnim);

        if(stunAnim)
        Anim3_ReleaseEnd();

        EventM.OnStunned(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();
    }

    // ============================================================================
    
    [Header("After Recover")]
    public Timer cooldown;
    public float cooldownTime=.5f;

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
