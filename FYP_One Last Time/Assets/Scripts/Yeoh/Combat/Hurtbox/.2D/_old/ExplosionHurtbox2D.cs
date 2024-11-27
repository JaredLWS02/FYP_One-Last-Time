using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox2D : MonoBehaviour
{
    public HurtboxSO hurtboxSO;
    
    // ============================================================================

    bool explodeOnAwake=true;

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        if(explodeOnAwake)
        Explode();
    }

    // ============================================================================

    [Header("Range")]
    public float outerRange=5;
    public float pushRangeMult=1.75f;
    public LayerMask layerMask;

    // ============================================================================

    public void Explode()
    {
        Damage();
        Push();
        CameraManager.Current.Shake();
    }

    // ============================================================================

    Collider2D[] GetOverlap(float range)
    {
        return Physics2D.OverlapCircleAll(transform.position, range, layerMask);
    }

    List<Rigidbody2D> GetRigidbodies(float range)
    {
        List<Rigidbody2D> rbs = new();

        Collider2D[] others = GetOverlap(range);

        foreach(Collider2D other in others)
        {
            if(other.isTrigger) continue;
            Rigidbody2D otherRb = other.attachedRigidbody;
            if(otherRb) rbs.Add(otherRb);
        }
        return rbs;
    }

    // ============================================================================

    Vector3 contactPoint;

    void Damage()
    {
        List<Rigidbody2D> rbs = GetRigidbodies(outerRange);

        foreach(var rb in rbs)
        {
            float falloffMult = GetFallOffMult(transform.position, rb.transform.position, outerRange);

            HurtboxSO new_hurtbox = HurtboxSO.CreateInstance(hurtboxSO);
            
            new_hurtbox.damage *= falloffMult;
            new_hurtbox.damageBlock *= falloffMult;
            new_hurtbox.stunSeconds *= falloffMult;
            new_hurtbox.knockback=0; // handled by Push()

            contactPoint = rb.ClosestPoint(transform.position);

            EventM.OnTryHurt(rb.gameObject, gameObject, new_hurtbox, contactPoint);
        }
    }

    // ============================================================================

    void Push()
    {
        List<Rigidbody2D> rbs = GetRigidbodies(outerRange * pushRangeMult);

        foreach(var rb in rbs)
        {
            Vector3 push_dir = (rb.transform.position - transform.position).normalized;

            float falloffMult = GetFallOffMult(transform.position, rb.transform.position, outerRange);

            float knockback = hurtboxSO.knockback * falloffMult;

            rb.velocity=Vector3.zero;
            rb.AddForce(knockback * push_dir * falloffMult, ForceMode2D.Impulse);
        }
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
