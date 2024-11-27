using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallScript : MonoBehaviour
{
    public GameObject owner;
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
        UpdateMaxFall();

        UpdateDebug();
    }

    // ============================================================================

    [Header("Fast Falling")]
    public bool fastFall=true;
    public float minFastFallVelocity = -1;
    public float fastFallForce = -30;

    bool currentlyFastFalling=false;

    bool IsFalling() => rb.velocity.y<0;

    bool IsFastFalling()
    {
        if(!IsFalling()) return false;

        bool isFastFalling = rb.velocity.y < minFastFallVelocity;

        if(isFastFalling)
        {
            if(!currentlyFastFalling)
            {
                currentlyFastFalling=true;
                fallEvents.ToggleFastFall?.Invoke(true);
                EventM.OnToggleFastFall(owner, true);
            }
        }
        else
        {
            if(currentlyFastFalling)
            {
                currentlyFastFalling=false;
                fallEvents.ToggleFastFall?.Invoke(false);
                EventM.OnToggleFastFall(owner, false);
            }
        }
        return isFastFalling;
    }

    void UpdateFastFalling()
    {
        if(!fastFall) return;
        if(!IsFastFalling()) return;

        rb.AddForce(Vector3.up * fastFallForce);
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
    public struct FallEvents
    {
        public UnityEvent<bool> ToggleFastFall;
    }
    
    [Header("Unity Events")]
    public FallEvents fallEvents;

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
