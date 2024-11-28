using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    void OnFirstEnter(GameObject obj, Collider coll)
    {
        // going down only
        //if(rb.velocity.y <= minLandVelocity)
        if(rb.velocity.y <= 0)
        {
            landAnim?.Play(owner);

            EventM.OnLandGround(owner);
            events.OnLandGround?.Invoke();
        }
    }

    void OnLastExit(GameObject obj, Collider coll)
    {
        EventM.OnLeaveGround(owner);
        events.OnLeaveGround?.Invoke();
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

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnLandGround;
        public UnityEvent OnLeaveGround;
    }
    [Space]
    public Events events;
}
