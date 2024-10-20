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

        UpdateDebug();
    }

    // ============================================================================

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
    public float minVelocityBeforeFastFall = -.1f;
    public float fastFallForce = -30;

    void UpdateFastFalling()
    {
        if(!fastFall) return;
        // ignore if going up
        if(rb.velocity.y>=0) return;
        
        if(rb.velocity.y < minVelocityBeforeFastFall)
        {
            rb.AddForce(Vector3.up * fastFallForce);
        }
    }

    // ============================================================================

    [Header("Debug")]
    public float y_velocity;

    void UpdateDebug()
    {
        y_velocity = Round(rb.velocity.y, 2);
    }

    public float Round(float num, int decimalPlaces)
    {
        int factor=1;

        for(int i=0; i<decimalPlaces; i++)
        {
            factor *= 10;
        }

        return Mathf.Round(num * factor) / factor;
    }
}
