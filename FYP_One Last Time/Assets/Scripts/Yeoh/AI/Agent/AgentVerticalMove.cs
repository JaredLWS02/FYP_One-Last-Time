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
    
    [Header("Check")]
    public float checkHeight=2.5f;

    protected override void OnSlowUpdate()
    {
        if(vehicle.goal)
        CheckHeight(vehicle.goal.position);
    }

    void CheckHeight(Vector3 target_pos)
    {
        if(!vehicle.goal) return;
        if(!InRange(target_pos)) return;

        float y_dist = Mathf.Abs(target_pos.y - owner.transform.position.y);

        if(y_dist < checkHeight)
        {
            sideMove.yInputOverride = null;
            return;
        }

        bool isAbove = target_pos.y > owner.transform.position.y;

        if(isAbove)
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
    
    public float checkRange=100;

    bool InRange(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, owner.transform.position);
        return distance <= checkRange;
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
