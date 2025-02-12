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
    
    public float checkRange=6;

    bool InRange(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, owner.transform.position);
        return distance <= checkRange;
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = new(0,1,0,.25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(owner.transform.position, checkRange);
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.up * checkHeight);
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Vector3.down * checkHeight);
    }
}
