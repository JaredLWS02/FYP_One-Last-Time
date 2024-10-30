using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================

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
        if(isAttacking) return;

        if(IsCooling()) return;
        DoCooldown();

        Attack();
    }

    // During Attack ============================================================================

    void Attack()
    {
        if(attackSO.noAnim)
        {
            AttackRelease();
        }
        else
        {
            attackSO.anim.Play(owner);
        }

        EventM.OnAttacked(owner, attackSO);
    }

    // ============================================================================

    public bool isWindingUp {get; private set;}
    public bool isAttacking {get; private set;}
    
    public bool IsAttacking()
    {
        return isAttacking || isWindingUp;
    }

    // Attack Anim Events ============================================================================

    public void AttackWindUp()
    {
        isWindingUp=true;
        isAttacking=false;

        if(attackSO.dashOnWindUp)
        Dash(attackSO.dashOnWindUpForce, attackSO.dashOnWindUpDir);
    }  

    public void AttackRelease()
    {
        isWindingUp=false;
        isAttacking=true;

        SpawnAttack();

        if(attackSO.dashOnRelease)
        Dash(attackSO.dashOnReleaseForce, attackSO.dashOnReleaseDir);

        EventM.OnAttackReleased(owner, attackSO);
    }

    public void AttackRecover()
    {
        isWindingUp=false;
        isAttacking=false;

        if(attackSO.dashOnRecover)
        Dash(attackSO.dashOnRecoverForce, attackSO.dashOnRecoverDir);
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

    // After Attack ============================================================================

    void Update()
    {
        UpdateCooldown();
    }

    // ============================================================================

    float cooldownLeft;
    
    void DoCooldown()
    {
        cooldownLeft = attackSO.cooldownTime;
    }

    void UpdateCooldown()
    {
        // only tick down if not busy
        if(IsAttacking()) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling()
    {
        return cooldownLeft>0;
    }

    void CancelCooldown()
    {
        cooldownLeft=0;
    }

    // Cancel ============================================================================
    
    void OnCancelAttack(GameObject who)
    {
        if(who!=owner) return;

        if(!IsAttacking()) return;

        AttackRecover();

        attackSO.anim.Cancel(owner);

        EventM.OnAttackCancelled(owner);
    }
}
