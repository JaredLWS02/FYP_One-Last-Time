using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class FallScript : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdateFastFalling();
        UpdateMaxFall();
    }

    // ============================================================================

    [Min(0)]
    public float maxFallVelocity = -30;

    void UpdateMaxFall()
    {
        if(rb.velocity.y < maxFallVelocity)
        {
            rb.velocity = new(rb.velocity.x, maxFallVelocity, rb.velocity.z);
            return;
        }
    }

    // ============================================================================

    [Header("Fast Falling")]
    public bool fastFall = true;
    [Min(0)]
    public float minVelocityBeforeFastFall = -.1f;
    [Min(0)]
    public float fastFallForce = 15;

    void UpdateFastFalling()
    {
        if(!fastFall) return;
        // ignore if going up
        if(rb.velocity.y>=0) return;
        
        if(rb.velocity.y < minVelocityBeforeFastFall)
        {
            rb.AddForce(Vector3.down * fastFallForce);
        }
    }
}
