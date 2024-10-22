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

        isAttacking=true;

        SpawnAttack();
    }

    void OnAttackRecover(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        isAttacking=false;
    }  

    // ============================================================================
    
    // triggered by other scripts

    public AttackSO attackSO;
    public PrefabSpawn attackSpawn;
    
    public void PlayAttackAnim()
    {
        isAttacking=true;
        EventManager.Current.OnPlayAnim(gameObject, attackSO.animName, attackSO.animLayer, attackSO.animBlendTime);
    }

    public void SpawnAttack()
    {
        Vector3 position = attackSpawn.spawnpoint.position;

        Quaternion rotation = attackSpawn.followRotation ? attackSpawn.spawnpoint.rotation : Quaternion.identity;

        GameObject spawned = Instantiate(attackSpawn.prefab, position, rotation);

        if(attackSpawn.parented) spawned.transform.parent = attackSpawn.spawnpoint;

        if(attackSO.dashOnRelease)
            Dash();
    }

    void Dash()
    {
        Vector3 direction = attackSO.dashDirection.normalized;

        if(attackSO.localDirection)
        direction = transform.TransformDirection(direction);

        rb.AddForce(attackSO.dashForce * direction, ForceMode.Impulse);
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
