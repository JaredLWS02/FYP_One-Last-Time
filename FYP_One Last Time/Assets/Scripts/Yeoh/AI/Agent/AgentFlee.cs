using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentVelocity))]

public class AgentFlee : MonoBehaviour
{
    NavMeshAgent agent;
    AgentVelocity agentVel;

    public Transform fleeGoal;

    Vector3 goalPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentVel = GetComponent<AgentVelocity>();

        fleeGoal.parent=null;

        goalPos = agent.transform.position;
    }

    void FixedUpdate()
    {
        fleeGoal.position = goalPos;

        CheckFlee();
    }

    // ============================================================================

    public Transform threat;
    public float range=10;

    public Vector3 axisMult = Vector3.one;

    void CheckFlee()
    {
        // ignore if main agent's goal is not the flee goal
        if(agentVel.goal != fleeGoal) return;

        if(!threat) return;

        if(!IsInRange(threat, range)) return;

        Vector3 threat_to_agent_dir = (agent.transform.position - threat.position).normalized;
        
        Vector3 offset_pos = threat_to_agent_dir * range;

        Vector3 flee_spot = agent.transform.position + offset_pos;

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
    
    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = Color.yellow;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
