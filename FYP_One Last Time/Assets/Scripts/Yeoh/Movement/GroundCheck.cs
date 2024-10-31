using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : SphereOverlap
{
    [Header("Ground Check")]
    public Rigidbody rb;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================
    
    //public float minLandVelocity = -1;
    
    public override void OnOverlapFirstEnter(Collider other)
    {
        // going down only
        //if(rb.velocity.y <= minLandVelocity)
        if(rb.velocity.y <= 0)
        {
            EventM.OnLandGround(gameObject);
        }
    }

    public override void OnOverlapLastExit(Collider other)
    {
        EventM.OnLeaveGround(gameObject);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return IsOverlapping();
    }
}
