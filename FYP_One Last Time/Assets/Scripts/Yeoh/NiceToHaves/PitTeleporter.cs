using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PitTeleporter : SlowUpdate
{
    [Header("Pit Teleporter")]
    public GameObject owner;
    public GroundCheck ground;

    Vector3 lastGroundedPos;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.PitTeleportEvent += OnPitTeleport;
    }
    protected override void OnDisable()
    {
        EventM.PitTeleportEvent -= OnPitTeleport;
    }

    // ============================================================================

    void Awake()
    {
        Record();
    }

    // ============================================================================

    protected override void OnSlowUpdate()
    {
        if(ground.IsGrounded())
            Record();

        if(owner.transform.position.y < minY)
            EventM.OnPitTeleport(owner);
    }

    void Record() => lastGroundedPos = owner.transform.position;

    // ============================================================================

    [Header("Pit")]
    public float minY = -20;

    public void OnPitTeleport(GameObject who)
    {
        if(who != owner) return;
        
        lastGroundedPos = SnapToNavMesh(lastGroundedPos);
        owner.transform.position = lastGroundedPos;
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
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = new(1, 0, 0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!owner) return;

        Vector3 start = owner.transform.position;
        Vector3 end = new
        (
            owner.transform.position.x,
            minY,
            owner.transform.position.z
        );

        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(start, end);
    }
}
