using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentReturn : MonoBehaviour
{
    public GameObject owner;
    public NavMeshAgent agent;

    // ============================================================================
    
    [Header("Return")]
    public Transform spawnpoint;

    void Awake()
    {
        spawnpoint.parent = null;
        spawnpoint.position = SnapToNavMesh(spawnpoint.position);
    }

    // ============================================================================

    public float checkInterval=.5f;
    float cooldownLeft;

    void DoCooldown() => cooldownLeft = checkInterval;

    void UpdateCooldown()
    {
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling() => cooldownLeft>0;
    
    // ============================================================================

    public bool shouldReturn {get; private set;}

    public void UpdateCheck(Vector3 chasedown_pos)
    {
        UpdateCooldown();
        if(IsCooling()) return;
        DoCooldown();

        // ignore if cant find a way back
        // or if still close to target
        if(!IsPathable(spawnpoint.position) || CanStillChaseDown(chasedown_pos))
        {
            shouldReturn=false;
            return;
        }
                
        shouldReturn = !IsNearSpawnpoint();
    }

    // ============================================================================
    
    bool IsInRange(Vector3 from, Vector3 target, float range)
    {
        return Vector2.Distance(from, target) <= range;
    }

    public float maxChaseDownRange=7;

    bool CanStillChaseDown(Vector3 chasedown_pos)
    {
        return IsInRange(owner.transform.position, chasedown_pos, maxChaseDownRange);
    }

    public float returnRange=20;

    bool IsNearSpawnpoint()
    {
        return IsInRange(spawnpoint.position, owner.transform.position, returnRange);
    }

    public bool IsAtSpawnpoint(float stopping_range)
    {
        return IsInRange(spawnpoint.position, owner.transform.position, stopping_range);
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

    bool IsPathable(Vector3 pos)
    {
        NavMeshPath path = new();
        agent.CalculatePath(pos, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;

    public bool showMaxChaseDownRangeGizmo = true;
    public bool showReturnRangeGizmo = true;

    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showMaxChaseDownRangeGizmo)
        Gizmos.DrawWireSphere(owner.transform.position, maxChaseDownRange);

        if(showReturnRangeGizmo)
        {
            Vector3 spawn_pos = Application.isPlaying ? spawnpoint.position : owner.transform.position;
            Gizmos.DrawWireSphere(spawn_pos, returnRange);
        }
    }
}
