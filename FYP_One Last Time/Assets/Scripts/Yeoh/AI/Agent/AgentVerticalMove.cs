using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVerticalMove : SlowUpdate
{
    [Header("Vertical")]
    public GameObject owner;
    public AgentVehicle vehicle;
    public AgentSideMove sideMove;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    protected override void OnSlowUpdate()
    {
        if(!ShouldDoVerticalMovement(vehicle.goal))
        {
            sideMove.yInputOverride = null;
            return;
        }

        bool is_above = vehicle.goal.position.y > owner.transform.position.y;

        if(is_above)
        {
            EventM.OnAgentTryJump(owner); // jump duh

            sideMove.yInputOverride = 1; // press up to climb
        }
        else
        {
            EventM.OnAgentTryJumpCut(owner); // jumpcut

            sideMove.yInputOverride = -1; // press down to descend platforms
        }
    }
    
    // ============================================================================
    
    [Header("Check")]
    public float checkRange = 10;
    public float checkHeight = 2.5f;

    bool ShouldDoVerticalMovement(Transform target)
    {
        if(!target) return false;

        Vector3 vector = target.position - owner.transform.position;

        float distance = vector.magnitude;
        if(distance > checkRange) return false;

        bool has_height_difference = Mathf.Abs(vector.y) >= checkHeight;
        return has_height_difference;
    }

    // ============================================================================

    [System.Serializable]
    public struct GizmoGroup
    {
        public bool showGizmos;
        public GizmoOptions rangeGizmo;
        public GizmoOptions heightGizmo;
    }

    [System.Serializable]
    public struct GizmoOptions
    {
        public bool show;
        public Color color;
    }

    [Space]
    public GizmoGroup gizmos;

    void OnDrawGizmosSelected()
    {
        if(!gizmos.showGizmos) return;

        if(gizmos.rangeGizmo.show)
        {
            Gizmos.color = gizmos.rangeGizmo.color;
            Gizmos.DrawWireSphere(owner.transform.position, checkRange);
        }
            
        if(gizmos.heightGizmo.show)
        {
            Gizmos.color = gizmos.heightGizmo.color;
            Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.up * checkHeight);
            Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.down * checkHeight);
        }
    }
}
