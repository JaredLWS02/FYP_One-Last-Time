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

    [Header("Rays")]
    public Transform upRayOrigin;
    public float upRayRange=2;
    public Transform downRayOrigin;
    public float downRayRange=1;
    public LayerMask hitLayers;
    
    [Header("Step Up")]
    public bool doStepUp=true;
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
        if(!doStepUp) return;
        if(isDeplatforming) return;
        if(!IsMoving()) return;

        if(ground && !ground.IsGrounded()) return;
        
        if(!IsUpRayValid()) return;
        if(!IsDownRayValid()) return;

        //rb.velocity = new(rb.velocity.x, 0, rb.velocity.z);
        if(rb.velocity.magnitude < upSpeed)
        rb.AddForce(upRayOrigin.up * upSpeed*10);

        stepUpEvents.StepUpUpdateEvent?.Invoke();
    }

    bool IsUpRayValid()
    {
        return !Physics.Raycast(upRayOrigin.position, upRayOrigin.up, upRayRange, hitLayers, QueryTriggerInteraction.Ignore);
    }

    bool IsDownRayValid()
    {
        if(Physics.Raycast(downRayOrigin.position, -downRayOrigin.up, out RaycastHit hit, downRayRange, hitLayers, QueryTriggerInteraction.Ignore))
        {
            if(!slope) return true;

            float angle = slope.GetSlopeAngle(hit.normal);
            return !slope.IsSlope(angle);
        }
        return false;
    }
    
    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;

    public Color upRayInvalidColor = new(1,0,0, .5f);
    public Color upRayValidColor = new(0,1,0, .5f);

    public Color downRayInvalidColor = new(1,1,1, .5f);
    public Color downRayValidColor = new(0,1,0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!upRayOrigin) return;
        if(!downRayOrigin) return;
        
        Gizmos.color = IsUpRayValid() ? upRayValidColor : upRayInvalidColor;
        DrawRayGizmo(upRayOrigin.position, upRayOrigin.up, upRayRange);

        Gizmos.color = IsDownRayValid() ? downRayValidColor : downRayInvalidColor;
        DrawRayGizmo(downRayOrigin.position, -downRayOrigin.up, downRayRange);
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
