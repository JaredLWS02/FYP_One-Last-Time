using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepUpCheck : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.MoveEvent += OnMove;
        EventM.DeplatformEvent += OnDeplatform;
    }
    void OnDisable()
    {
        EventM.MoveEvent -= OnMove;
        EventM.DeplatformEvent -= OnDeplatform;
    }

    // ============================================================================

    Vector2 moveInput;

    void OnMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;

        moveInput = input;
    }

    bool IsMoving() => moveInput != Vector2.zero;

    // ============================================================================

    void OnDeplatform(GameObject who, Collider platform, bool toggle)
    {
        if(who!=owner) return;

        isDeplatforming = toggle;
    }

    // ============================================================================
    
    [Header("Parent")]
    public Transform moveDirection;

    [Header("Ray")]
    public Transform rayOrigin;
    public float rayRange=1;
    public float minHeightSpaceAboveStep=2;
    public LayerMask hitLayers;
    
    [Header("Step Up")]
    public bool doStepUpMove=true;
    public float upSpeed=5;
    bool isDeplatforming;

    [Header("Optional")]
    public SlopeCheck slope;
    public GroundCheck ground;

    // ============================================================================
    
    void FixedUpdate()
    {
        UpdateMoveDir();
        TryStepUp();
    }

    void UpdateMoveDir()
    {
        if(rb.velocity==Vector3.zero) return;

        Vector3 moveDir = rb.velocity;
        moveDir.y = 0;

        if(moveDir==Vector3.zero) return;

        moveDirection.forward = moveDir.normalized;
    }
    
    void TryStepUp()
    {
        if(isDeplatforming) return;
        if(!IsMoving()) return;

        if(ground && !ground.IsGrounded()) return;
        
        if(!CanStepUp()) return;

        stepUpEvents.StepUpUpdateEvent?.Invoke();

        if(doStepUpMove)
        {
            //rb.velocity = new(rb.velocity.x, 0, rb.velocity.z);
            if(rb.velocity.magnitude < upSpeed)
            rb.AddForce(owner.transform.up * upSpeed*10);
        }
    }

    RaycastHit hit;

    bool CanStepUp()
    {
        if(Physics.Raycast(rayOrigin.position, -rayOrigin.up, out hit, rayRange, hitLayers, QueryTriggerInteraction.Ignore))
        {
            if(!HasSpaceAbove(hit.point)) return false;

            if(!slope) return true;

            float angle = slope.GetSlopeAngle(hit.normal);
            return !slope.IsSlope(angle);
        }
        return false;
    }

    bool HasSpaceAbove(Vector3 pos)
    {
        return !Physics.Raycast(pos, rayOrigin.up, minHeightSpaceAboveStep, hitLayers, QueryTriggerInteraction.Ignore);
    }
    
    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color rayColor = new(1,1,1, .5f);
    public Color rayValidColor = new(0,1,0, .5f);
    public Color rayInvalidColor = new(1,0,0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!rayOrigin) return;
        
        bool canStepUp = CanStepUp();

        Gizmos.color = canStepUp ? rayValidColor : rayColor;
        DrawRayGizmo(rayOrigin.position, -rayOrigin.up, rayRange);

        if(!canStepUp) return;

        Gizmos.color = HasSpaceAbove(hit.point) ? rayColor : rayInvalidColor;
        DrawRayGizmo(hit.point, rayOrigin.up, minHeightSpaceAboveStep);
    }

    void DrawRayGizmo(Vector3 start, Vector3 dir, float range)
    {
        Vector3 end = start + dir * range;
        Gizmos.DrawLine(start, end);
    }

    // ============================================================================

    [System.Serializable]
    public struct StepUpEvents
    {
        public UnityEvent StepUpUpdateEvent;
    }
    [Space]
    public StepUpEvents stepUpEvents;
}
