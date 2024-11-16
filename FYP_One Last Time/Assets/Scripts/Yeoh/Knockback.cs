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
    public float knockbackMult=1;

    public void OnTryKnockback(GameObject who, float force, Vector3 contactPoint)
    {
        if(who!=owner) return;

        if(!allowKnockback) return;
        if(knockbackMult==0) return;

        if(rb.isKinematic) return;

        Vector3 kb_dir = (rb.transform.position - contactPoint).normalized;

        if(knockbackMult!=1)
        force *= knockbackMult;

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * force, ForceMode.Impulse);
    }
}
