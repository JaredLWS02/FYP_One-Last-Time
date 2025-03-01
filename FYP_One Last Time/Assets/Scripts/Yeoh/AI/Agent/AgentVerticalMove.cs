using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVerticalMove : SlowUpdate
{
    [Header("Vertical")]
    public GameObject owner;
    public AgentVehicle vehicle;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================
    
    [Header("Check")]
    public float checkHeight=2.5f;

    public override void OnSlowUpdate()
    {
        if(vehicle.goal)
        CheckHeight(vehicle.goal.position);
    }

    void CheckHeight(Vector3 target_pos)
    {
        if(!vehicle.goal) return;
        if(!InRange(target_pos)) return;

        float y_dist = Mathf.Abs(target_pos.y - owner.transform.position.y);
        if(y_dist < checkHeight) return;

        bool isAbove = target_pos.y > owner.transform.position.y;

        if(isAbove)
        {
            EventM.OnAgentTryJump(owner); // jump duh
            EventM.OnAgentTryMove(owner, Vector2.up); // press up
        }
        else
        {
            EventM.OnAgentTryJumpCut(owner); // jumpcut
            EventM.OnAgentTryMove(owner, Vector2.down); // press down
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
    public struct Debug
    {
        public bool showGizmos;
        public Gizmo rangeGizmo;
        public Gizmo heightGizmo;
    }

    [System.Serializable]
    public struct Gizmo
    {
        public bool show;
        public Color color;
    }

    [Space]
    public Debug debug;

    void OnDrawGizmosSelected()
    {
        if(!debug.showGizmos) return;

        if(debug.rangeGizmo.show)
        {
            Gizmos.color = debug.rangeGizmo.color;
            Gizmos.DrawWireSphere(owner.transform.position, checkRange);
        }
            
        if(debug.heightGizmo.show)
        {
            Gizmos.color = debug.heightGizmo.color;
            Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.up * checkHeight);
            Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.down * checkHeight);
        }
    }
}
