using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Rigidbody rb;

    // ============================================================================
    
    EventManager EventM;

    public OverlapScript overlap;

    void OnEnable()
    {
        EventM = EventManager.Current;

        overlap.FirstEnterEvent += OnFirstEnter;
        overlap.LastExitEvent += OnLastExit;
    }
    void OnDisable()
    {
        overlap.FirstEnterEvent -= OnFirstEnter;
        overlap.LastExitEvent -= OnLastExit;
    }

    // ============================================================================
    
    //public float minLandVelocity = -1;
    
    void OnFirstEnter(Collider other)
    {
        // going down only
        //if(rb.velocity.y <= minLandVelocity)
        if(rb.velocity.y <= 0)
        {
            EventM.OnLandGround(gameObject);
        }
    }

    void OnLastExit(Collider other)
    {
        EventM.OnLeaveGround(gameObject);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return overlap.IsOverlapping();
    }
}
