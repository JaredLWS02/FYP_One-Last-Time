using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public GameObject owner;
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
    
    [Header("Land")]
    public AnimSO landAnim;
    //public float minLandVelocity = -1;

    void OnFirstEnter(Collider other)
    {
        // going down only
        //if(rb.velocity.y <= minLandVelocity)
        if(rb.velocity.y <= 0)
        {
            landAnim?.Play(owner);

            EventM.OnLandGround(owner);
        }
    }

    void OnLastExit(Collider other)
    {
        EventM.OnLeaveGround(owner);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return overlap.IsOverlapping();
    }

    // ============================================================================

    [Header("Animator")]
    public Animator anim;
    public string groundedBoolName = "IsGrounded";

    void FixedUpdate()
    {
        anim?.SetBool(groundedBoolName, IsGrounded());
    }
}
