using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnParryScript : BaseAction
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ParryEvent += OnParry;
        EventM.CancelParryEvent += OnCancelParry;
    }
    void OnDisable()
    {
        EventM.ParryEvent -= OnParry;
        EventM.CancelParryEvent -= OnCancelParry;
    }

    // ============================================================================

    [Header("On Parry")]
    public AnimSO parryAnim;

    public bool counterStun=true;
    public bool counterKnockback=true;
    public bool selfKnockback=true;

    [Header("Optional")]
    public AnimSO parryCustomStunAnim;

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        Perform(parryAnim);
        Anim3_ReleaseEnd();

        if(counterStun && hurtbox.parryStunsOwner)
        {
            // choose the parry's unique stun anim
            if(parryCustomStunAnim)
            hurtbox.customStunAnim = parryCustomStunAnim;

            EventM.OnStun(attacker, owner, hurtbox, contactPoint);
        }

        if(counterKnockback)
        CounterKnockback(attacker, hurtbox, contactPoint);

        if(selfKnockback)
        EventM.OnTryKnockback(owner, hurtbox.blockKnockback, contactPoint);
    }

    void CounterKnockback(GameObject who, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        float counter_knockback = hurtbox.knockback - hurtbox.blockKnockback;

        counter_knockback = Mathf.Max(counter_knockback, hurtbox.blockKnockback);

        EventM.OnTryKnockback(who, counter_knockback, contactPoint);
    }

    // Cancel ============================================================================

    void OnCancelParry(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();

        EventM.OnParryCancelled(owner);
    }
}
