using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class AttackScript : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    public void DoBuffer()
    {
        bufferLeft = bufferTime;
    }

    void UpdateBuffer()
    {
        bufferLeft -= Time.deltaTime;

        if(bufferLeft<0) bufferLeft=0;
    }

    bool HasBuffer()
    {
        return bufferLeft>0;
    }

    void ResetBuffer()
    {
        bufferLeft=0;
    }

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
        EventManager.Current.OnPlayAnim(gameObject, attackSO.animName, attackSO.animLayer, attackSO.animBlendTime);
    }

    void DoInstantAttack()
    {
        EventManager.Current.OnAttackRelease(gameObject);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.AttackWindUpEvent += OnAttackWindUp;
        EventManager.Current.AttackReleaseEvent += OnAttackRelease;
        EventManager.Current.AttackRecoverEvent += OnAttackRecover;
    }
    void OnDisable()
    {
        EventManager.Current.AttackWindUpEvent -= OnAttackWindUp;
        EventManager.Current.AttackReleaseEvent -= OnAttackRelease;
        EventManager.Current.AttackRecoverEvent -= OnAttackRecover;
    }

    // ============================================================================

    // triggered by attack anim events

    public bool isAttacking {get; private set;}

    void OnAttackWindUp(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        isAttacking=true;

        if(attackSO.dashOnWindUp)
            Dash();
    }  

    void OnAttackRelease(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        SpawnAttack();

        EventManager.Current.OnAttack(gameObject, attackSO);
    }

    void OnAttackRecover(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        isAttacking=false;
    }  

    // ============================================================================

    public void SpawnAttack()
    {
        Quaternion rotation = attackSpawn.followRotation ? attackSpawn.spawnpoint.rotation : Quaternion.identity;

        GameObject spawned = Instantiate(attackSpawn.prefab, attackSpawn.spawnpoint.position, rotation);

        if(attackSpawn.parented) spawned.transform.parent = attackSpawn.spawnpoint;

        if(attackSO.dashOnRelease)
            Dash();
    }

    void Dash()
    {
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
    
    [Header("Cancel")]
    public string cancelAnimName = "Cancel";

    public void CancelAttackAnim()
    {
        if(!isAttacking) return;

        EventManager.Current.OnPlayAnim(gameObject, cancelAnimName, attackSO.animLayer, attackSO.animBlendTime);

        EventManager.Current.OnAttackRecover(gameObject);
    }
}
