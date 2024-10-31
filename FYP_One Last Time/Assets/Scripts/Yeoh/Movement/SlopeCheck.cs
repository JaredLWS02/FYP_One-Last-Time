using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;
    public GroundCheck ground;

    // ============================================================================
    
    [Header("Ray")]
    public Transform rayOrigin;
    public float rayLength=1;
    public LayerMask layers;
    RaycastHit rayHit;
    
    [Header("Slope")]
    public float maxSlopeAngle=35;
    bool onSlope;
    public bool isTooSteep {get; private set;}
    Vector3 slopeDir;

    void CheckSlope()
    {
        if(Physics.Raycast(rayOrigin.position, -rayOrigin.up, out rayHit, rayLength, layers))
        {
            float angle = Vector3.Angle(Vector3.up, rayHit.normal);
            
            onSlope = angle!=0 && ground.IsGrounded();
            
            isTooSteep = angle > maxSlopeAngle;

            slopeDir = Vector3.Cross(rayHit.normal, Vector3.Cross(Vector3.up, rayHit.normal));
        }
    }

    // ============================================================================
    
    void FixedUpdate()
    {
        CheckSlope();

        if(isTooSteep)
        {
            CounterClimb();
        }
        else if(onSlope)
        {
            CounterSlide();
        }
    }

    // ============================================================================

    public float climbCounterForce=500;

    void CounterClimb()
    {
        if(rb.velocity.y<0) return;

        rb.AddForce(-slopeDir * climbCounterForce, ForceMode.Force);
    }
    
    public float slideCounterForce=60;

    void CounterSlide()
    {
        if(rb.velocity.y>=0) return;

        rb.AddForce(slopeDir * slideCounterForce, ForceMode.Force);
    }

    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color onSlopeColor = Color.green;
    public Color noSlopeColor = Color.yellow;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!rayOrigin) return;

        Vector3 start = rayOrigin.position;
        Vector3 end = start + -rayOrigin.up * rayLength;

        Gizmos.color = onSlope && isTooSteep ? onSlopeColor : noSlopeColor;
        Gizmos.DrawLine(start, end);
    }
}
