using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRaycast : BaseRaycast
{
    [Header("Sphere Cast")]
    public float radius=1;
    float base_radius;

    public override bool IsOriginHit(out Collider[] overlaps)
    {
        overlaps = Physics.OverlapSphere(origin.position, radius, hitLayers, QueryTriggerInteraction.Ignore);
        return overlaps.Length>0;
    }

    public override bool IsRayBlocked(out RaycastHit hit)
    {
        return Physics.SphereCast(origin.position, radius, origin.forward, out hit, range, hitLayers, QueryTriggerInteraction.Ignore);
    }

    public override bool IsRayHit(out GameObject ray_obj)
    {
        if(IsRayBlocked(out var hit))
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
        base_radius = radius;
    }

    public override void OnBaseSetDefault()
    {
        radius = base_radius;
    }

    // ============================================================================
        
    public override void OnBaseDrawRayGizmos(Vector3 start, Vector3 end)
    {
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
    }

    public override void OnBaseDrawOriginGizmos(Vector3 origin){}
}
