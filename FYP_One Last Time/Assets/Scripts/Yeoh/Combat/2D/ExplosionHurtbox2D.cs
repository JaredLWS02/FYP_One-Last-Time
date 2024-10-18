using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox2D : MonoBehaviour
{
    bool explodeOnAwake=true;

    void Enable()
    {
        if(explodeOnAwake)
        Explode();
    }

    // ============================================================================

    [Header("Explode")]
    public float maxDamage=5;
    public LayerMask layerMask;
    public float outerRange=5;
    public float force=10;
    public float forceRangeMult=1.5f;

    // ============================================================================

    public void Explode()
    {
        Damage();
        Push();
        CameraManager.Current.Shake();
    }

    // ============================================================================

    List<Rigidbody2D> GetRigidbodies(float range)
    {
        List<Rigidbody2D> rbs = new();

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, layerMask);

        foreach(Collider2D other in others)
        {
            if(other.isTrigger) continue;
            Rigidbody2D otherRb = other.attachedRigidbody;
            if(otherRb) rbs.Add(otherRb);
        }
        return rbs;
    }

    // ============================================================================

    float fallOffDamage;
    Vector3 contactPoint;

    void Damage()
    {
        List<Rigidbody2D> rbs = GetRigidbodies(outerRange);

        foreach(var rb in rbs)
        {
            float falloffMult = GetFallOffMult(transform.position, rb.transform.position, outerRange);

            fallOffDamage = maxDamage * falloffMult;

            contactPoint = rb.ClosestPoint(transform.position);

            EventManager.Current.OnHit(gameObject, rb.gameObject, CopyHurtInfo());
        }
    }

    // ============================================================================

    HurtInfo2D CopyHurtInfo()
    {
        return new()
        {
            damage = fallOffDamage,
            contactPoint = contactPoint,
        };
    }

    // ============================================================================

    void Push()
    {
        List<Rigidbody2D> rbs = GetRigidbodies(outerRange * forceRangeMult);

        foreach(var rb in rbs)
        {
            Vector3 push_dir = (rb.transform.position - transform.position).normalized;

            float falloffMult = GetFallOffMult(transform.position, rb.transform.position, outerRange);

            rb.velocity=Vector3.zero;
            rb.AddForce(force * push_dir * falloffMult, ForceMode2D.Impulse);
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
        Gizmos.DrawWireSphere(transform.position, outerRange * forceRangeMult);
    }
}
