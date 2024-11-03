using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : BaseAction
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.CancelAttackEvent += OnCancelAttack;
    }
    void OnDisable()
    {
        EventM.CancelAttackEvent -= OnCancelAttack;
    }

    // ============================================================================
    
    [Header("On Attack")]
    public AttackSO attackSO;
    [Space]
    public PrefabPreset attackPrefab;
    
    public void TryAttack()
    {
        if(IsPerforming()) return;

        if(IsCooling()) return;

        Perform(attackSO.anim);

        EventM.OnAttacked(owner, attackSO);
    }

    // ============================================================================

    // Anim Event
    public override void OnAnimWindUp()
    {
        if(attackSO.dashOnWindUp)
        Dash(attackSO.dashOnWindUpForce, attackSO.dashOnWindUpDir);

        EventM.OnAttackWindedUp(owner, attackSO);
    }  
    // Anim Event
    public override void OnAnimReleaseStart()
    {
        if(attackSO.dashOnRelease)
        Dash(attackSO.dashOnReleaseForce, attackSO.dashOnReleaseDir);

        SpawnAttack();

        EventM.OnAttackReleased(owner, attackSO);
    }
    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();
    }  

    // ============================================================================

    public void SpawnAttack()
    {
        GameObject spawned = attackPrefab.Spawn();

        TryAssignHurtboxOwner(spawned);
    }

    void TryAssignHurtboxOwner(GameObject target)
    {
        if(target.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            hurtbox.owner = owner;
        }
        if(target.TryGetComponent<ExplosionHurtbox>(out var e_hurtbox))
        {
            e_hurtbox.owner = owner;
        }
    }

    // ============================================================================
    
    [Header("Attack Dash")]
    public Rigidbody rb;

    void Dash(float force, Vector3 dir)
    {
        if(!rb) return;
        
        if(force==0) return;
        if(dir==Vector3.zero) return;

        Vector3 direction = dir.normalized;

        if(attackSO.localDir)
        direction = transform.TransformDirection(direction);

        rb.AddForce(force * direction, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("After Attack")]
    public Timer cooldown;

    void DoCooldown() => cooldown?.StartTimer(attackSO.cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================
    
    void OnCancelAttack(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();

        EventM.OnAttackCancelled(owner);
    }
}
