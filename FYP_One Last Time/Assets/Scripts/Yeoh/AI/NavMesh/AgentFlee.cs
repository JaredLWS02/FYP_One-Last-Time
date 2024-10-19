using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class AgentFlee : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform goal;

    Vector3 goalPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        goal.parent=null;

        goalPos = agent.transform.position;
    }

    void OnEnable()
    {
        InvokeRepeating(nameof(CheckFlee), 0, checkInterval);
    }

    void FixedUpdate()
    {
        goal.position = goalPos;
    }

    // ============================================================================

    public Transform threat;
    public float fleeRange=10;

    public Vector3 axisMult = Vector3.one;

    public float checkInterval=.1f;

    void CheckFlee()
    {
        if(!threat) return;

        if(!IsInRange(threat, fleeRange)) return;

        Vector3 threat_to_agent_dir = (agent.transform.position - threat.position).normalized;
        
        Vector3 offset_pos = threat_to_agent_dir * fleeRange;

        Vector3 flee_spot = agent.transform.position + offset_pos;

        flee_spot = SnapToNavMesh(flee_spot);

        flee_spot.Scale(axisMult); // same as multiply xyz

        goalPos = flee_spot;
    }

    // ============================================================================

    bool IsInRange(Transform target, float range)
    {
        float distance = Vector3.Distance(agent.transform.position, target.position);
        return distance <= range;
    }

    // ============================================================================

    Vector3 SnapToNavMesh(Vector3 pos)
    {
        if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 9999, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return agent.transform.position;
    }
    
    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.yellow;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, fleeRange);
    }
}
