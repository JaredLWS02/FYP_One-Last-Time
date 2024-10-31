using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================

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

        if(IsTryingToParry()) return;

        if(IsCooling()) return;
        DoCooldown();

        RaiseParry();
    }

    // ============================================================================

    [Header("Try Parry")]
    public AnimSO tryParryAnim;
    
    void RaiseParry()
    {
        // action cancelling
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelCast(owner);
        EventM.OnCancelStun(owner);

        tryParryAnim.Play(owner);
        
        EventM.OnRaisedParry(owner);
    }

    // ============================================================================
    
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
        return IsFacing(attack_pos) && isRaisingParry;
    }

    // on hit parry
    void OnTryParry(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(CanParry(contactPoint) && hurtbox.isParryable)
        {
            EventM.OnParry(owner, attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
        }
    }

    // ============================================================================

    bool isRaisingParry;
    bool isLoweringParry;

    public bool IsTryingToParry()
    {
        return isRaisingParry || isLoweringParry;
    }

    // ============================================================================
    
    // Anim Event
    public void ParryRaise()
    {
        isRaisingParry=true;
        isLoweringParry=false;
    }
    // Anim Event
    public void ParryLower()
    {
        isRaisingParry=false;
        isLoweringParry=true;
    }
    // Anim Event
    public void ParryLowered()
    {
        isRaisingParry=false;
        isLoweringParry=false;
    }
    // Note: DO NOT PLAY/CANCEL ANY ANIMATIONS IN ON EXIT
    // OTHER ANIMATIONS MIGHT TRY TO TAKE OVER, THUS TRIGGERING ON EXIT,
    // IF GOT ANY PLAY/CANCEL ANIM ON EXIT, IT WILL REPLACE IT

    // ============================================================================

    void Update()
    {
        UpdateCooldown();
        UpdateRiposte();
    }

    // ============================================================================

    [Header("After Lower Parry")]
    public float cooldownTime=.3f;
    float cooldownLeft;
    
    void DoCooldown() => cooldownLeft = cooldownTime;

    void UpdateCooldown()
    {
        // only tick down if not busy
        if(IsTryingToParry()) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling() => cooldownLeft>0;

    void CancelCooldown() => cooldownLeft=0;
    
    // ============================================================================
    
    [Header("On Parry")]
    public AnimSO parryAnim;
    public bool counterStun=true;
    public bool counterKnockback=true;
    public bool selfKnockback=true;

    [Header("Optional")]
    public AnimSO parryCustomStunAnim;

    public bool isParrying {get; private set;}

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        ParryLowered();

        CancelCooldown();

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

        Parry();

        StartRiposte();
    }

    void CounterKnockback(GameObject who, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        float counter_knockback = hurtbox.knockback - hurtbox.blockKnockback;

        counter_knockback = Mathf.Max(counter_knockback, hurtbox.blockKnockback);

        EventM.OnTryKnockback(who, counter_knockback, contactPoint);
    }

    void Parry()
    {
        isParrying=true;

        parryAnim.Play(owner);
    }

    // ============================================================================
    
    // Anim Event
    public void ParryRecover()
    {
        isParrying=false;
    }
    // Note: DO NOT PLAY/CANCEL ANY ANIMATIONS IN ON EXIT
    // OTHER ANIMATIONS MIGHT TRY TO TAKE OVER, THUS TRIGGERING ON EXIT,
    // IF GOT ANY PLAY/CANCEL ANIM ON EXIT, IT WILL REPLACE IT

    // ============================================================================
    
    [Header("Riposte")]
    public float riposteActiveSeconds=.5f;
    float riposteLeft;

    void StartRiposte() => riposteLeft = riposteActiveSeconds;

    void UpdateRiposte()
    {
        riposteLeft -= Time.deltaTime;

        if(riposteLeft<0) riposteLeft=0;
    }

    public bool IsRiposteActive() => riposteLeft>0;

    void CancelRiposte() => riposteLeft=0;

    // Cancel ============================================================================

    void OnCancelParry(GameObject who)
    {
        if(who!=owner) return;

        if(IsTryingToParry())
        {
            ParryLowered();

            tryParryAnim.Cancel(owner);

            EventM.OnParryCancelled(owner);
        }

        if(isParrying)
        {
            ParryRecover();

            parryAnim.Cancel(owner);

            CancelRiposte();

            EventM.OnParryCancelled(owner);
        }
    }
}
