using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================
    
    EventManager EventM;
    public BaseOverlap overlap;

    void OnEnable()
    {
        EventM = EventManager.Current;

        overlap.OverlapFirstEnterEvent += OnFirstEnter;
        overlap.OverlapLastExitEvent += OnLastExit;
    }
    void OnDisable()
    {
        overlap.OverlapFirstEnterEvent -= OnFirstEnter;
        overlap.OverlapLastExitEvent -= OnLastExit;
    }

    // ============================================================================
    
    [Header("Land")]
    public AnimSO landAnim;
    //public float minLandVelocity = -1;

    void OnFirstEnter(GameObject who)
    {
        // going down only
        //if(rb.velocity.y <= minLandVelocity)
        if(rb.velocity.y <= 0)
        {
            landAnim?.Play(owner);

            EventM.OnLandGround(owner);
        }
    }

    void OnLastExit(GameObject who)
    {
        EventM.OnLeaveGround(owner);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        bool isOverlapping = overlap.IsOverlapping();

        if(slope)
        {
            return isOverlapping && !slope.isTooSteep;
        }
        return isOverlapping;
    }

    // ============================================================================

    [Header("Animator")]
    public Animator anim;
    public string groundedBoolName = "IsGrounded";

    void FixedUpdate()
    {
        anim?.SetBool(groundedBoolName, IsGrounded());
    }

    // ============================================================================

    [Header("Optional")]
    public SlopeCheck slope;
}
