using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHurtbox : BaseHurtbox
{
    [Header("Collider Hurtbox")]
    public Collider myColl;

    public bool enabledOnAwake=true;

    void Awake()
    {
        ToggleCollider(enabledOnAwake);
    }
    
    // ============================================================================
    
    public enum HitMethod
    {
        OnEnter,
        OnStay,
        OnExit,
    }
    [Header("OnHit")]
    public HitMethod hitMethod = HitMethod.OnStay;

    void OnTriggerEnter(Collider other)
    {
        if(hitMethod != HitMethod.OnEnter) return;

        TryHit(other);
    }
    
    public Timer triggerTimer;
    public float triggerStayInterval=.1f;

    void OnTriggerStay(Collider other)
    {
        if(hitMethod != HitMethod.OnStay) return;
        
        if(triggerTimer)
        {
            if(triggerTimer.IsTicking()) return;
            triggerTimer.StartTimer(triggerStayInterval);
        }

        TryHit(other);
    }
    
    void OnTriggerExit(Collider other)
    {
        if(hitMethod != HitMethod.OnExit) return;

        TryHit(other);
    }

    // ============================================================================
    
    public Transform hurtboxOrigin;

    void TryHit(Collider other)
    {
        if(!IsColliderValid(other, out var obj)) return;

        contactPoint = other.ClosestPoint(hurtboxOrigin.position);

        Hit(obj);
    }
    
    // ============================================================================
    
    public bool ignoreTriggers=true;
    public bool onlyRigidbodies=true;
    
    protected bool IsColliderValid(Collider target_coll, out GameObject obj)
    {
        if(ignoreTriggers && target_coll.isTrigger)
        {
            obj=null;
            return false;
        }

        Rigidbody rb = target_coll.attachedRigidbody;

        if(onlyRigidbodies && !rb)
        {
            obj=null;
            return false;
        }

        obj = rb ? rb.gameObject : target_coll.gameObject;
        return true;
    }
    
    // ============================================================================

    public void ToggleCollider(bool toggle) => myColl.enabled = toggle;

    protected override void OnCancelAttack()
    {
        ToggleCollider(false);
    }

    // Old ============================================================================

    public void Blink(float time=.1f)
    {
        if(time<=0) return;
        if(blinking_crt!=null) StopCoroutine(blinking_crt);
        blinking_crt = StartCoroutine(Blinking(time));
    }

    Coroutine blinking_crt;

    IEnumerator Blinking(float t)
    {
        ToggleCollider(true);
        yield return new WaitForSeconds(t);
        ToggleCollider(false);
    }
}
