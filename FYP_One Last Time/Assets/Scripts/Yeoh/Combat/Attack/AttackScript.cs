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
    public PrefabSpawn attackSpawn;
    
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
        if(attackSO.hasAttackAnim)
        {
            PlayAttackAnim();
        }
        else
        {
            DoInstantAttack();
        }

        EventM.OnAttacked(owner, attackSO);
    }

    void PlayAttackAnim()
    {
        EventM.OnPlayAnim(owner, attackSO.animName, attackSO.animLayer, attackSO.animBlendTime);
    }

    void DoInstantAttack()
    {
        AttackRelease();
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
            Dash();
    }  

    public void AttackRelease()
    {
        isWindingUp=false;
        isAttacking=true;

        SpawnAttack();

        EventM.OnAttackReleased(owner, attackSO);
    }

    public void AttackRecover()
    {
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

        EventM.OnAttackCancelled(owner);

        PlayCancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Attack";
    
    void PlayCancelAnim()
    {
        EventM.OnPlayAnim(owner, cancelAnimName, attackSO.animLayer, attackSO.animBlendTime);
    }   
}
