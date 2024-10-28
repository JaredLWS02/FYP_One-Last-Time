using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    // ============================================================================

    public bool explodeOnAwake=true;

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

    Collider[] GetOverlap(float range)
    {
        return Physics.OverlapSphere(transform.position, range, layerMask);
    }

    List<Rigidbody> GetRigidbodies(float range)
    {
        List<Rigidbody> rbs = new();

        Collider[] others = GetOverlap(range);

        foreach(Collider other in others)
        {
            if(other.isTrigger) continue;
            Rigidbody otherRb = other.attachedRigidbody;
            if(otherRb) rbs.Add(otherRb);
        }
        return rbs;
    }

    List<Collider> GetColliders(float range)
    {
        List<Collider> colls = new();

        Collider[] others = GetOverlap(range);

        foreach(Collider other in others)
        {
            if(other.isTrigger) continue;
            Rigidbody otherRb = other.attachedRigidbody;
            if(otherRb) colls.Add(other);
        }
        return colls;
    }

    // ============================================================================

    public HurtboxSO hurtboxSO;

    Vector3 contactPoint;

    void Damage()
    {
        List<Collider> others = GetColliders(outerRange);

        foreach(var other in others)
        {
            Rigidbody otherRb = other.attachedRigidbody;

            float falloffMult = GetFallOffMult(transform.position, otherRb.transform.position, outerRange);

            HurtboxSO new_hurtbox = HurtboxSO.CreateInstance(hurtboxSO);

            new_hurtbox.damage *= falloffMult;
            new_hurtbox.blockDamage *= falloffMult;
            new_hurtbox.knockback = 0; // handled by Push()

            contactPoint = other.ClosestPoint(transform.position);
            
            EventM.OnTryHurt(otherRb.gameObject, owner, new_hurtbox, contactPoint);
        }
    }

    // ============================================================================

    void Push()
    {
        List<Rigidbody> rbs = GetRigidbodies(outerRange * pushRangeMult);

        foreach(var rb in rbs)
        {
            Vector3 push_dir = (rb.transform.position - transform.position).normalized;

            float falloffMult = GetFallOffMult(transform.position, rb.transform.position, outerRange);

            float knockback = hurtboxSO.knockback * falloffMult;

            rb.velocity=Vector3.zero;
            rb.AddForce(knockback * push_dir, ForceMode.Impulse);
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
