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
    public float rayLength=2;
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
            GetSlope(rayHit.normal, out float angle, out bool is_slope, out bool is_too_steep, out Vector3 slope_dir);

            onSlope = ground.IsGrounded() && is_slope;
            isTooSteep = is_too_steep;
            slopeDir = slope_dir;
        }
    }

    public void GetSlope(Vector3 normal, out float angle, out bool is_slope, out bool is_too_steep, out Vector3 slope_dir)
    {
        angle = GetSlopeAngle(normal);
        is_slope = IsSlope(angle);
        is_too_steep = IsTooSteep(angle);
        slope_dir = GetSlopeDir(normal);
    }

    public float GetSlopeAngle(Vector3 normal) => Vector3.Angle(Vector3.up, normal);
    public bool IsSlope(float angle) => angle!=0;
    public bool IsTooSteep(float angle) => angle > maxSlopeAngle;
    public Vector3 GetSlopeDir(Vector3 normal) => Vector3.Cross(normal, Vector3.Cross(Vector3.up, normal));

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
