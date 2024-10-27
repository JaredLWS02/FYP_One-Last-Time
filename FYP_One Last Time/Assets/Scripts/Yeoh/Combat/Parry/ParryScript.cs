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
        
        EventM.ParryRecoverEvent += OnParryRecover;

        EventM.ParrySuccessEvent += OnParrySuccess;
        EventM.CancelParryEvent += OnCancelParry;
    }
    void OnDisable()
    {
        EventM.ParryRecoverEvent -= OnParryRecover;

        EventM.ParrySuccessEvent -= OnParrySuccess;
        EventM.CancelParryEvent -= OnCancelParry;
    }

    // ============================================================================

    void Update()
    {
        UpdateBuffer();

        TryRaiseParry();
        UpdateRaise();

        UpdateCooldown();

        UpdateRiposte();
    }

    // ============================================================================

    [Header("Before Parry")]
    public float bufferTime=.2f;
    float bufferLeft;

    public void DoBuffer() => bufferLeft = bufferTime;

    void UpdateBuffer()
    {
        bufferLeft -= Time.deltaTime;

        if(bufferLeft<0) bufferLeft=0;
    }

    bool HasBuffer() => bufferLeft>0;

    void ResetBuffer() => bufferLeft=0;

    // On Raise Parry ============================================================================
        
    void TryRaiseParry()
    {
        if(IsParrying()) return;

        if(!HasBuffer()) return;

        if(IsCooling()) return;

        RaiseParry();
    }

    // ============================================================================

    void RaiseParry()
    {
        ResetBuffer();

        DoCooldown();

        // attack cancelling
        EventM.OnCancelAttack(owner);
        // ability cancelling
        EventM.OnCancelCast(owner);

        StartRaise();

        PlayRaiseParryAnim();
    }

    // ============================================================================

    [Header("Parry Anim")]
    public int parryAnimLayer;
    public float parryAnimBlendTime; 

    [Header("Raise Parry Anim")]
    public string raiseParryAnimName = "Raise Parry";

    void PlayRaiseParryAnim()
    {
        EventM.OnPlayAnim(owner, raiseParryAnimName, parryAnimLayer, parryAnimBlendTime);
    }

    // ============================================================================

    public bool isParryRaised {get; private set;}
    public bool isParryLowering {get; private set;}

    bool IsParrying()
    {
        return isParryRaised || isParryLowering;
    }

    // ============================================================================

    [Header("During Raised Parry")]
    public float raiseParrySeconds=.3f;
    float raiseLeft;

    public void StartRaise()
    {
        isParryRaised=true;

        raiseLeft = raiseParrySeconds;
    }

    void UpdateRaise()
    {
        if(!isParryRaised) return;

        raiseLeft -= Time.deltaTime;

        if(raiseLeft<0) raiseLeft=0;

        if(!HasRaise())
        {
            isParryRaised=false;

            ResetRaise();

            LowerParry();
        }
    }

    bool HasRaise() => raiseLeft>0;

    void ResetRaise() => raiseLeft=0;

    // Lower Parry ============================================================================

    void LowerParry()
    {
        isParryLowering=true;

        PlayLowerParryAnim();
    }

    // ============================================================================

    [Header("Lower Parry Anim")]
    public string lowerParryAnimName = "Lower Parry";

    void PlayLowerParryAnim()
    {
        EventM.OnPlayAnim(owner, lowerParryAnimName, parryAnimLayer, parryAnimBlendTime);
    }

    // Parry Anim Events ============================================================================

    void OnParryRecover(GameObject who)
    {
        if(who!=owner) return;

        isParryRaised=false;
        isParryLowering=false;
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
    
    void OnParrySuccess(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        EventM.OnCancelParry(defender);

        if(hurtbox.parryStunsAttacker)
        {
            // stop attacker's attack first or else cant stun because state machine
            EventM.OnCancelAttack(attacker);
            EventM.OnTryStun(attacker, owner, hurtbox, contactPoint);
            EventM.OnKnockback(attacker, hurtbox.blockKnockback, contactPoint);
        }

        EventM.OnKnockback(owner, hurtbox.blockKnockback, contactPoint);

        StartRiposte();
    }

    [Header("On Parry Success")]
    public float riposteSeconds=.3f;
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

        CancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Parry";
    
    void CancelAnim()
    {
        if(!IsParrying()) return;

        EventM.OnParryRecover(owner);

        CancelCooldown();

        EventM.OnPlayAnim(owner, cancelAnimName, parryAnimLayer, parryAnimBlendTime);
    }       
}
