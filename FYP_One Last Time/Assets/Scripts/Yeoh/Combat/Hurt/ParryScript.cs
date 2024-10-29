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
        
        EventM.ParryEvent += OnParry;
        EventM.ParrySuccessEvent += OnParrySuccess;
        EventM.CancelParryEvent += OnCancelParry;
    }
    void OnDisable()
    {
        EventM.ParryEvent -= OnParry;
        EventM.ParrySuccessEvent -= OnParrySuccess;
        EventM.CancelParryEvent -= OnCancelParry;
    }

    // ============================================================================
        
    void OnParry(GameObject who)
    {
        if(who!=owner) return;

        if(IsParrying()) return;

        if(IsCooling()) return;
        DoCooldown();

        Parry();
    }

    // ============================================================================

    [Header("Raise Parry Anim")]
    public AnimPreset raiseParryAnim;
    
    void Parry()
    {
        // action cancelling
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);
        EventM.OnCancelStun(owner);

        raiseParryAnim.Play(owner);
        
        EventM.OnRaisedParry(owner);
    }

    // ============================================================================

    public bool isParryRaised {get; private set;}
    public bool isParryLowering {get; private set;}

    bool IsParrying()
    {
        return isParryRaised || isParryLowering;
    }

    // ============================================================================
    
    // Anim Event
    public void ParryRaise()
    {
        isParryRaised=true;
        isParryLowering=false;
    }
    // Anim Event
    public void ParryLower()
    {
        isParryRaised=false;
        isParryLowering=true;
    }
    // Anim Event
    public void ParryRecover()
    {
        isParryRaised=false;
        isParryLowering=false;
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
        if(IsParrying()) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling() => cooldownLeft>0;

    void CancelCooldown() => cooldownLeft=0;

    // ============================================================================
    
    [Header("On Try Parry")]
    [Range(-1,1)]
    public float minParryDot=0.2f;

    public bool IsFacing(Vector3 target_pos)
    {
        Vector3 dir_to_target = (target_pos - owner.transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, dir_to_target);

        return dot > minParryDot;
    }
    
    // ============================================================================
    
    [Header("On Parry Success")]
    public AnimPreset parrySuccessAnim;
    [Space]
    public AnimPreset parryStunAnim;

    public bool isParrySuccessing {get; private set;}

    void OnParrySuccess(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        ParryRecover();

        CancelCooldown();

        if(hurtbox.parryStunsOwner)
        {
            // choose the parry's unique stun anim
            hurtbox.stunAnim = parryStunAnim;

            EventM.OnStun(attacker, owner, hurtbox, contactPoint);

            EventM.OnKnockback(attacker, hurtbox.knockback, contactPoint);
        }

        EventM.OnKnockback(owner, hurtbox.blockKnockback, contactPoint);

        ParrySuccess();

        StartRiposte();
    }

    void ParrySuccess()
    {
        isParrySuccessing=true;

        parrySuccessAnim.Play(owner);
    }

    // ============================================================================
    
    // Anim Event
    public void ParrySuccessRecover()
    {
        isParrySuccessing=false;
    }
    // Note: DO NOT PLAY/CANCEL ANY ANIMATIONS IN ON EXIT
    // OTHER ANIMATIONS MIGHT TRY TO TAKE OVER, THUS TRIGGERING ON EXIT,
    // IF GOT ANY PLAY/CANCEL ANIM ON EXIT, IT WILL REPLACE IT

    // ============================================================================
    
    [Header("Riposte")]
    public float riposteSeconds=.5f;
    float riposteLeft;

    void StartRiposte() => riposteLeft = riposteSeconds;

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

        if(IsParrying())
        {
            ParryRecover();

            raiseParryAnim.Cancel(owner);

            EventM.OnParryCancelled(owner);
        }

        if(isParrySuccessing)
        {
            ParrySuccessRecover();

            parrySuccessAnim.Cancel(owner);

            CancelRiposte();

            EventM.OnParryCancelled(owner);
        }
    }
}
