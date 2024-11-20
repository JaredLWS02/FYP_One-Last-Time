using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryKnockbackEvent += OnTryKnockback;
    }
    void OnDisable()
    {
        EventM.TryKnockbackEvent -= OnTryKnockback;
    }

    // ============================================================================

    public Rigidbody rb;
    public bool allowKnockback=true;
    public Vector3Int knockbackAxis = new(1,0,1);
    public float knockbackMult=1;

    public void OnTryKnockback(GameObject who, float force, Vector3 contactPoint)
    {
        if(who!=owner) return;

        if(!allowKnockback) return;
        if(knockbackMult==0) return;
        if(rb.isKinematic) return;

        Vector3 center = ColliderManager.Current.GetCenter(owner);

        Vector3 kb_dir = center - contactPoint;
        if(knockbackAxis.x<=0) kb_dir.x=0;
        if(knockbackAxis.y<=0) kb_dir.y=0;
        if(knockbackAxis.z<=0) kb_dir.z=0;
        kb_dir = kb_dir.normalized;

        if(knockbackMult!=1)
        force *= knockbackMult;

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * force, ForceMode.Impulse);
    }
}
