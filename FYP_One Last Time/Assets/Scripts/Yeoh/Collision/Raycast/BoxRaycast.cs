using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRaycast : BaseRaycast
{
    [Header("Box Cast")]
    public Vector3 size = Vector3.one;
    Vector3 base_size;

    public override bool RayHit(out GameObject target)
    {
        if(Physics.BoxCast(origin.position, size*.5f, GetRayDir(), out rayHit, origin.rotation, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            if(IsHitValid(out var hitObj))
            {
                target = hitObj;
                return true;
            }
        }
        target=null;
        return false;
    }

    // ============================================================================
    
    public override void OnBaseAwake()
    {
        base_size = size;
    }

    public override void OnSetDefault()
    {
        size = base_size;
    }

    // ============================================================================
        
    public override void OnBaseDrawGizmos(Vector3 start, Vector3 end)
    {
        Gizmos.matrix = Matrix4x4.TRS(start, origin.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = Matrix4x4.TRS(end, origin.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        // Reset the Gizmo matrix to default
        // (to avoid affecting other gizmos)
        Gizmos.matrix = Matrix4x4.identity;
    }
}
