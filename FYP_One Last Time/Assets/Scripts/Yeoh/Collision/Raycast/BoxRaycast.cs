using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRaycast : BaseRaycast
{
    [Header("Box Cast")]
    public Vector3 size = Vector3.one;
    Vector3 base_size;

    public override bool IsOriginHit(out Collider[] overlaps)
    {
        overlaps = Physics.OverlapBox(origin.position, size*.5f, origin.rotation, hitLayers, QueryTriggerInteraction.Ignore);
        return overlaps.Length>0;
    }

    public override bool IsRayHit(out GameObject ray_obj)
    {
        if(Physics.BoxCast(origin.position, size*.5f, origin.forward, out var hit, origin.rotation, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            rayHit = GetRayHit(hit);
            
            return IsColliderValid(rayHit.collider, out ray_obj);
        }
        ray_obj=null;
        return false;
    }

    // ============================================================================
    
    public override void OnBaseAwake()
    {
        base_size = size;
    }

    public override void OnBaseSetDefault()
    {
        size = base_size;
    }

    // ============================================================================
        
    public override void OnBaseDrawRayGizmos(Vector3 start, Vector3 end)
    {
        Gizmos.matrix = Matrix4x4.TRS(start, origin.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = Matrix4x4.TRS(end, origin.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        // Reset the Gizmo matrix to default
        // (to avoid affecting other gizmos)
        Gizmos.matrix = Matrix4x4.identity;
    }

    public override void OnBaseDrawOriginGizmos(Vector3 origin){}
}
