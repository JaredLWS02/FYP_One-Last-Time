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

    void Awake()
    {
        Record();
    }

    public override void OnSlowUpdate()
    {
        if(ground.IsGrounded())
            Record();

        if(owner.transform.position.y < minY)
            Teleport();
    }

    void Record() => lastGroundedPos = owner.transform.position;

    // ============================================================================

    [Header("Pit")]
    public float minY = -20;

    void Teleport()
    {
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
}
