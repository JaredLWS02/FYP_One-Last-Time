using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class AgentReturn : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform spawnpoint;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        spawnpoint.parent = null;
        spawnpoint.position = SnapToNavMesh(spawnpoint.position);
    }

    // ============================================================================

    public float maxChaseDownRange=7;
    public float returnRange=20;

    public bool ShouldReturn(Vector3 target_pos)
    {
        // ignore if cant find a way back
        if(!IsPathable(spawnpoint.position)) return false;

        // ignore if still close to target
        if(IsInRange(agent.transform.position, target_pos, maxChaseDownRange)) return false;

        return !IsInRange(spawnpoint.position, transform.position, returnRange);
    }

    public bool IsAtSpawnpoint(float stopping_range)
    {
        return IsInRange(spawnpoint.position, agent.transform.position, stopping_range);
    }

    // ============================================================================
    
    bool IsInRange(Vector3 from, Vector3 target, float range)
    {
        return Vector2.Distance(from, target) <= range;
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

    bool IsPathable(Vector3 pos)
    {
        NavMeshPath path = new();
        agent.CalculatePath(pos, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos = true;

    public bool showMaxChaseDownRangeGizmo = true;
    public bool showReturnRangeGizmo = true;

    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showMaxChaseDownRangeGizmo)
        Gizmos.DrawWireSphere(transform.position, maxChaseDownRange);

        if(showReturnRangeGizmo)
        {
            Vector3 spawn_pos = Application.isPlaying ? spawnpoint.position : transform.position;
            Gizmos.DrawWireSphere(spawn_pos, returnRange);
        }
    }
}
