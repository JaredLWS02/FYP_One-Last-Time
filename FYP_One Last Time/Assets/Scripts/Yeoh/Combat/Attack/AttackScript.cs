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
        
        EventM.AttackWindUpEvent += OnAttackWindUp;
        EventM.AttackReleaseEvent += OnAttackRelease;
        EventM.AttackRecoverEvent += OnAttackRecover;

        EventM.CancelAttackEvent += OnCancelAttack;
    }
    void OnDisable()
    {
        EventM.AttackWindUpEvent -= OnAttackWindUp;
        EventM.AttackReleaseEvent -= OnAttackRelease;
        EventM.AttackRecoverEvent -= OnAttackRecover;

        EventM.CancelAttackEvent -= OnCancelAttack;
    }

    // ============================================================================

    void Update()
    {
        UpdateBuffer();
        TryAttack();

        UpdateCooldown();
    }

    // ============================================================================

    [Header("Before Attack")]
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

    // ============================================================================
    
    [Header("On Attack")]
    public AttackSO attackSO;
    public PrefabSpawn attackSpawn;
    
    void TryAttack()
    {
        if(isAttacking) return;

        if(!HasBuffer()) return;

        if(IsCooling()) return;

        Attack();
    }

    // During Attack ============================================================================

    void Attack()
    {
        ResetBuffer();

        DoCooldown();

        if(attackSO.hasAttackAnim)
        {
            StartAttackAnim();
        }
        else
        {
            DoInstantAttack();
        }
    }

    void StartAttackAnim()
    {
        EventM.OnPlayAnim(owner, attackSO.animName, attackSO.animLayer, attackSO.animBlendTime);
    }

    void DoInstantAttack()
    {
        EventM.OnAttackRelease(owner);
    }

    // Attack Anim Events ============================================================================

    public bool isWindingUp {get; private set;}
    public bool isAttacking {get; private set;}

    void OnAttackWindUp(GameObject attacker)
    {
        if(attacker!=owner) return;

        isWindingUp=true;
        isAttacking=false;

        if(attackSO.dashOnWindUp)
            Dash();
    }  

    void OnAttackRelease(GameObject attacker)
    {
        if(attacker!=owner) return;

        isWindingUp=false;
        isAttacking=true;

        SpawnAttack();

        EventM.OnAttack(owner, attackSO);
    }

    void OnAttackRecover(GameObject attacker)
    {
        if(attacker!=owner) return;

        isWindingUp=false;
        isAttacking=false;
    }  

    // ============================================================================

    public void SpawnAttack()
    {
        Quaternion rotation = attackSpawn.followRotation ? attackSpawn.spawnpoint.rotation : Quaternion.identity;

        GameObject spawned = Instantiate(attackSpawn.prefab, attackSpawn.spawnpoint.position, rotation);

        if(attackSpawn.parented) spawned.transform.parent = attackSpawn.spawnpoint;

        TryAssignHurtboxOwner(spawned);

        if(attackSO.dashOnRelease)
            Dash();
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

    void Dash()
    {
        if(!rb) return;
        
        if(attackSO.dashForce==0) return;
        if(attackSO.dashDirection==Vector3.zero) return;

        Vector3 direction = attackSO.dashDirection.normalized;

        if(attackSO.localDirection)
        direction = transform.TransformDirection(direction);

        rb.AddForce(attackSO.dashForce * direction, ForceMode.Impulse);
    }

    // After Attack ============================================================================

    float cooldownLeft;
    
    void DoCooldown()
    {
        cooldownLeft = attackSO.cooldownTime;
    }

    void UpdateCooldown()
    {
        // only tick down if not busy
        if(isAttacking) return;
        
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

    // ============================================================================
    
    public bool IsAttacking()
    {
        return isAttacking || isWindingUp;
    }
    
    // Cancel ============================================================================
    
    void OnCancelAttack(GameObject who)
    {
        if(who!=owner) return;

        CancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Attack";
    
    void CancelAnim()
    {
        if(!IsAttacking()) return;

        EventM.OnAttackRecover(owner);

        EventM.OnPlayAnim(owner, cancelAnimName, attackSO.animLayer, attackSO.animBlendTime);
    }   
}
