using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunScript : BaseAction
{
    [Header("Stun script")]
    public AnimSO defaultStunAnim;
    AnimSO currentStunAnim;

    void Awake()
    {
        currentStunAnim = defaultStunAnim;
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

        // action cancelling
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);

        actionCounter++;

        currentStunAnim = hurtbox.customStunAnim ?
            hurtbox.customStunAnim : defaultStunAnim;

        Perform(currentStunAnim);
        Anim3_ReleaseEnd();

        EventM.OnStunned(owner, attacker, hurtbox, contactPoint);
    }

    // Cancel ============================================================================

    void OnCancelStun(GameObject who)
    {
        if(who!=owner) return;
        
        if(!IsPerforming()) return;

        CancelAnim();
    }
}
