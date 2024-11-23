using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseHurtbox : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    // ============================================================================

    protected EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.HurtedEvent += OnHurted;
        EventM.AttackCancelledEvent += OnAttackCancelled;

        OnBaseEnable();
    }
    void OnDisable()
    {
        EventM.HurtedEvent -= OnHurted;
        EventM.AttackCancelledEvent -= OnAttackCancelled;

        OnBaseDisable();
    }

    protected virtual void OnBaseEnable(){}
    protected virtual void OnBaseDisable(){}

    // ============================================================================

    // on successful hit
    void OnHurted(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(owner != attacker) return;
        if(owner == victim) return;
        // this.hurtboxSO is NOT an instance
        // hurtbox param is an instance (clone)
        // they are different
        if(hurtboxSO.Name != hurtbox.Name) return;

        hurtbox.pierceCount--;
        if(hurtbox.pierceCount <= 0)
        OnCancelAttack();

        hurtboxEvents.OnHitSuccess?.Invoke(contactPoint);

        if(destroyOnHurt) Destroy(gameObject);
    }

    void OnAttackCancelled(GameObject who)
    {
        if(who!=owner) return;

        OnCancelAttack();
    }

    protected virtual void OnCancelAttack()
    {
        Destroy(gameObject);
    }
    
    // ============================================================================

    public HurtboxSO hurtboxSO;

    protected Vector3 contactPoint;

    public void Hit(GameObject who)
    {
        HurtboxSO new_hurtbox = HurtboxSO.CreateInstance(hurtboxSO);

        EventM.OnTryHurt(who, owner, new_hurtbox, contactPoint);

        hurtboxEvents.OnTryHit?.Invoke(contactPoint);

        if(destroyOnHit) Destroy(gameObject);
    }

    // ============================================================================

    [System.Serializable]
    public struct HurtboxEvents
    {
        public UnityEvent<Vector3> OnTryHit;
        public UnityEvent<Vector3> OnHitSuccess;
    }
    [Space]
    public HurtboxEvents hurtboxEvents;

    [Header("Optional")]
    public bool destroyOnHit;
    public bool destroyOnHurt;
}
