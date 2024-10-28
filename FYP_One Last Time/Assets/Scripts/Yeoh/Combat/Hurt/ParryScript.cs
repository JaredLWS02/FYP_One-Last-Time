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

        RaiseParry();
    }

    // ============================================================================

    [Header("Raise Parry Anim")]
    public AnimPreset raiseParryAnim;

    void RaiseParry()
    {
        // attack cancelling
        EventM.OnCancelAttack(owner);
        // ability cancelling
        EventM.OnCancelCast(owner);
        // stun cancelling
        EventM.OnCancelStun(owner);

        StartRaise();

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

    void Update()
    {
        UpdateRaise();
        UpdateCooldown();
        UpdateRiposte();
    }

    // ============================================================================
    
    [Header("During Raised Parry")]
    public float raiseParrySeconds=.3f;
    float raiseLeft;

    void StartRaise()
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

    [Header("Lower Parry Anim")]
    public AnimPreset lowerParryAnim;

    void LowerParry()
    {
        isParryLowering=true;

        lowerParryAnim.Play(owner);
    }

    // Parry Anim Events ============================================================================

    public void ParryRecover()
    {
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
    
    [Header("On Parry Success")]
    public AnimPreset parrySuccessAnim;

    void OnParrySuccess(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        EventM.OnCancelParry(defender);

        if(hurtbox.parryStunsAttacker)
        {
            EventM.OnStun(attacker, owner, hurtbox, contactPoint);

            EventM.OnKnockback(attacker, hurtbox.knockback, contactPoint);
        }

        EventM.OnKnockback(owner, hurtbox.blockKnockback, contactPoint);

        parrySuccessAnim.Play(owner);

        StartRiposte();
    }

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

        if(!IsParrying()) return;

        ParryRecover();

        CancelRiposte();

        raiseParryAnim.Cancel(owner);

        EventM.OnParryCancelled(owner);
    }
}
