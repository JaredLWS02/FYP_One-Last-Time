using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentFlee : MonoBehaviour
{
    public GameObject owner;
    public NavMeshAgent agent;
    public AgentVehicle vehicle;

    // ============================================================================
    
    [Header("Flee")]
    public Transform fleeGoal;

    Vector3 goalPos;

    void Awake()
    {
        fleeGoal.parent=null;

        goalPos = owner.transform.position;
    }

    // ============================================================================
    
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
        if(vehicle.goal != fleeGoal) return;

        if(!threat) return;

        if(!IsInRange(threat, range)) return;

        Vector3 threat_to_owner_dir = (owner.transform.position - threat.position).normalized;
        
        Vector3 offset_pos = threat_to_owner_dir * range;

        Vector3 flee_spot = owner.transform.position + offset_pos;

        flee_spot = SnapToNavMesh(flee_spot);

        flee_spot.Scale(axisMult); // same as multiply xyz

        goalPos = flee_spot;
    }

    // ============================================================================

    bool IsInRange(Transform target, float range)
    {
        float distance = Vector3.Distance(owner.transform.position, target.position);
        return distance <= range;
    }

    // ============================================================================

    Vector3 SnapToNavMesh(Vector3 pos)
    {
        if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 9999, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return owner.transform.position;
    }

    // ============================================================================
    
    public GameObject GetThreat() => threat.gameObject;

    public void SetThreat(Transform target)
    {
        if(!target) return;
        threat = target;
        SetGoalToFlee();
    }

    public void SetThreat(GameObject target)
    {
        if(!target) return;
        SetThreat(target.transform);
    }

    // ============================================================================
    
    [Header("Agent")]
    public float fleeArrivalRange=1;

    public void SetGoalToFlee()
    {
        vehicle.SetRange(fleeArrivalRange);
        vehicle.SetGoal(fleeGoal);
    }
        
    // ============================================================================

    void OnDestroy()
    {
        Destroy(fleeGoal.gameObject);
    }

    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = Color.yellow;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(owner.transform.position, range);
        Gizmos.DrawWireSphere(owner.transform.position, fleeArrivalRange);
    }
}
