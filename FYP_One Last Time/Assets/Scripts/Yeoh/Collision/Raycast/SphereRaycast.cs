using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRaycast : BaseRaycast
{
    [Header("Sphere Cast")]
    public float radius=1;
    float base_radius;

    public override bool HasHitTarget(out GameObject target)
    {
        if(Physics.SphereCast(rayOrigin.position, radius, GetRayDir(), out rayHit, range, hitLayers, QueryTriggerInteraction.Ignore))
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
        base_radius = radius;
    }

    public override void OnSetDefault()
    {
        radius = base_radius;
    }

    // ============================================================================
        
    public override void OnBaseDrawGizmos(Vector3 start, Vector3 end)
    {
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
    }
}
