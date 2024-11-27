using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox : BaseHurtbox
{
    [Header("Explosion Hurtbox")]
    public bool explodeOnAwake=true;

    void OnEnable()
    {
        if(explodeOnAwake) Explode();
    }

    // ============================================================================

    [Header("Range")]
    public float outerRange=5;
    public float pushRangeMult=1.75f;
    public LayerMask layerMask;

    Collider[] GetOverlap(float range) => Physics.OverlapSphere(transform.position, range, layerMask, QueryTriggerInteraction.Ignore);

    // ============================================================================

    public void Explode()
    {
        Damage();
        Push();
        CameraManager.Current.Shake();
    }

    // ============================================================================

    void Damage()
    {
        Collider[] others = GetOverlap(outerRange);

        foreach(var other in others)
        {
            if(!IsColliderValid(other, out var obj)) continue;

            float falloffMult = GetFallOffMult(transform.position, obj.transform.position, outerRange);

            HurtboxSO new_hurtbox = HurtboxSO.CreateInstance(hurtboxSO);

            new_hurtbox.damage *= falloffMult;
            new_hurtbox.blockDamage *= falloffMult;
            new_hurtbox.knockback = 0; // handled by Push()

            contactPoint = other.ClosestPoint(transform.position);
            
            EventM.OnTryHurt(obj, owner, new_hurtbox, contactPoint);
        }
    }

    // ============================================================================

    void Push()
    {
        Collider[] others = GetOverlap(outerRange);

        foreach(var other in others)
        {
            if(!IsColliderValid(other, out var obj)) continue;

            Vector3 push_dir = (obj.transform.position - transform.position).normalized;

            float falloffMult = GetFallOffMult(transform.position, obj.transform.position, outerRange);

            float knockback = hurtboxSO.knockback * falloffMult;

            Rigidbody otherRb = other.attachedRigidbody;

            otherRb.velocity=Vector3.zero;
            otherRb.AddForce(knockback * push_dir, ForceMode.Impulse);
        }
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

    float GetFallOffMult(Vector3 from, Vector3 to, float range)
    {
        float distance = Vector3.Distance(from, to);

        return 1 - (distance/range);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColorOuter = new(1, .5f, 0, 1);
    public Color gizmoColorPush = new(1, .5f, 0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColorOuter;
        Gizmos.DrawWireSphere(transform.position, outerRange);

        Gizmos.color = gizmoColorPush;
        Gizmos.DrawWireSphere(transform.position, outerRange * pushRangeMult);
    }
}
