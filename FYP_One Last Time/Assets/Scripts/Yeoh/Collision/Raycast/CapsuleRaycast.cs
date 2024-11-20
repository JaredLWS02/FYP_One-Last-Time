using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRaycast : BaseRaycast
{
    [Header("Capsule Cast")]
    public float radius=1;
    float base_radius;
    public float height=1;
    float base_height;

    public override bool IsOriginHit(out Collider[] overlaps)
    {
        GetCapsule(origin.position, out var top, out var bottom);

        overlaps = Physics.OverlapCapsule(top, bottom, radius, hitLayers, QueryTriggerInteraction.Ignore);
        return overlaps.Length>0;
    }

    public override bool IsRayHit(out GameObject ray_obj)
    {
        GetCapsule(origin.position, out var top, out var bottom);
        
        if(Physics.CapsuleCast(top, bottom, radius, origin.forward, out var hit, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            rayHit = GetRayHit(hit);

            return IsColliderValid(rayHit.collider, out ray_obj);
        }
        ray_obj=null;
        return false;
    }

    void GetCapsule(Vector3 pos, out Vector3 top, out Vector3 bottom)
    {
        top = pos + origin.up * height*.5f;
        bottom = pos + -origin.up * height*.5f;
    }

    // ============================================================================
    
    public override void OnBaseAwake()
    {
        base_radius = radius;
        base_height = height;
    }

    public override void OnBaseSetDefault()
    {
        radius = base_radius;
        height = base_height;
    }

    // ============================================================================
        
    public override void OnBaseDrawRayGizmos(Vector3 start, Vector3 end)
    {
        DrawWireCapsule(start);
        DrawWireCapsule(end);

        GetCapsule(start, out var top1, out var bottom1);
        GetCapsule(end, out var top2, out var bottom2);
        Gizmos.DrawLine(top1, top2);
        Gizmos.DrawLine(bottom1, bottom2);
    }

    void DrawWireCapsule(Vector3 pos)
    {
        GetCapsule(pos, out var top, out var bottom);

        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);
        Gizmos.DrawLine(top, bottom);
    }

    public override void OnBaseDrawOriginGizmos(Vector3 origin){}
}
