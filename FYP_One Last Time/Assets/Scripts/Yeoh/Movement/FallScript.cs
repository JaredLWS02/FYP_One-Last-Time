using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallScript : MonoBehaviour
{
    public Rigidbody rb;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    void FixedUpdate()
    {
        UpdateFastFalling();
        UpdateFastFallCheck();
        UpdateMaxFall();

        UpdateDebug();
    }

    // ============================================================================

    [Header("Fast Falling")]
    public bool fastFall = true;
    public float minFastFallVelocity = -1;
    public float fastFallForce = -30;
    bool fastFallStarted;

    bool IsFalling()
    {
        return rb.velocity.y<0;
    }

    bool IsFastFalling()
    {
        if(!IsFalling()) return false;

        return rb.velocity.y < minFastFallVelocity;
    }

    void UpdateFastFalling()
    {
        if(!fastFall) return;
        
        if(IsFastFalling())
        {
            rb.AddForce(Vector3.up * fastFallForce);
        }
    }

    void UpdateFastFallCheck()
    {
        if(!fastFallStarted && IsFastFalling())
        {
            fastFallStarted=true;
            uEvents.FastFallStart?.Invoke();
            EventM.OnFastFallStart(gameObject);
        }
        else if(fastFallStarted && !IsFalling())
        {
            fastFallStarted=false;
            uEvents.FastFallEnd?.Invoke();
            EventM.OnFastFallEnd(gameObject);
        }
    }

    // ============================================================================

    [Header("Max Velocity")]
    public float maxFallVelocity = -50;
    
    void UpdateMaxFall()
    {
        if(!IsFalling()) return;
        
        if(rb.velocity.y < maxFallVelocity)
        {
            rb.velocity = new(rb.velocity.x, maxFallVelocity, rb.velocity.z);
        }
    }

    // ============================================================================
    
    [System.Serializable]
    public struct UEvents
    {
        public UnityEvent FastFallStart;
        public UnityEvent FastFallEnd;
    }
    
    [Header("Unity Events")]
    public UEvents uEvents;

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
