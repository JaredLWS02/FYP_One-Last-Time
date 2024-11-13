using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryParryScript : BaseAction
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.RaiseParryEvent += OnRaiseParry;
        EventM.TryParryEvent += OnTryParry;
        EventM.ParryEvent += OnParry;
        EventM.CancelParryEvent += OnCancelParry;
    }
    void OnDisable()
    {
        EventM.RaiseParryEvent -= OnRaiseParry;
        EventM.TryParryEvent -= OnTryParry;
        EventM.ParryEvent -= OnParry;
        EventM.CancelParryEvent -= OnCancelParry;
    }

    // ============================================================================
        
    void OnRaiseParry(GameObject who)
    {
        if(who!=owner) return;

        if(IsPerforming()) return;

        if(IsCooling()) return;

        RaiseParry();
    }

    // ============================================================================

    [Header("Raise Parry")]
    public AnimSO raiseParryAnim;
    
    void RaiseParry()
    {
        // action cancelling
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        //EventM.OnCancelCast(owner);
        EventM.OnCancelStun(owner);

        Perform(raiseParryAnim);
        
        EventM.OnRaisedParry(owner);
    }

    // ============================================================================
    
    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();
    }

    // ============================================================================

    [Header("After Lower Parry")]
    public Timer cooldown;
    public float cooldownTime=.3f;

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();
    
    // ============================================================================
    
    [Header("On Try Parry")]
    [Range(-1,1)]
    public float minParryDot=0.2f;

    bool IsFacing(Vector3 target_pos)
    {
        Vector3 dir_to_target = (target_pos - owner.transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, dir_to_target);

        return dot > minParryDot;
    }

    public bool CanParry(Vector3 attack_pos)
    {
        return IsFacing(attack_pos) && IsReleasing();
    }

    public HurtScript hurt;

    // on hit parry
    void OnTryParry(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(hurt.iframe) return;

        if(CanParry(contactPoint) && hurtbox.isParryable)
        {
            EventM.OnParry(owner, attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
        }
    }

    // On Parry Success ============================================================================
    
    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        Anim4_Recover();

        CancelCooldown();
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
