using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepUpCheck : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.MoveEvent += OnMove;
    }
    void OnDisable()
    {
        EventM.MoveEvent -= OnMove;
    }

    // ============================================================================

    Vector2 moveInput;

    void OnMove(GameObject mover, Vector2 input)
    {
        if(owner!=mover) return;

        moveInput = input;
    }

    bool IsMoving() => moveInput != Vector2.zero;

    // ============================================================================
    
    [Header("Parent")]
    public Transform moveDirection;

    [Header("Rays")]
    public Transform upRayOrigin;
    public float upRayRange=2;
    public Transform downRayOrigin;
    public float downRayRange=1;
    public LayerMask hitLayers;
    
    [Header("Force")]
    public Rigidbody rb;
    public float upForce=50;

    [Header("Optional")]
    public SlopeCheck slope;
    public GroundCheck ground;

    // ============================================================================
    
    void FixedUpdate()
    {
        UpdateMoveDir();
        TryPushUp();
    }

    void UpdateMoveDir()
    {
        if(rb.velocity==Vector3.zero) return;

        Vector3 moveDir = rb.velocity;
        moveDir.y = 0;

        if(moveDir==Vector3.zero) return;

        moveDirection.forward = moveDir.normalized;
    }
    
    void TryPushUp()
    {
        if(!IsMoving()) return;

        if(ground && !ground.IsGrounded()) return;
        
        if(!IsUpRayValid()) return;
        if(!IsDownRayValid()) return;

        rb.velocity = Vector3.zero;
        rb.AddForce(owner.transform.up * upForce*10);
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
}
