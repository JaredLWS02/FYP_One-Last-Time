using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleOverlap : BaseOverlap
{
    public Transform orientation;

    public float radius=.5f;
    public float height=2;

    public override Collider[] GetOverlap()
    {
        float halfHeight = height*.5f;

        Vector3 point1 = origin.position + posOffset + orientation.up * -halfHeight;
        Vector3 point2 = origin.position + posOffset + orientation.up * halfHeight;

        return Physics.OverlapCapsule(point1, point2, radius, layers);
    }
    
    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = new(1, 1, 1, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!origin) return;
        if(!orientation) return;

        float halfHeight = height*.5f;

        Vector3 point1 = origin.position + posOffset + orientation.up * -halfHeight;
        Vector3 point2 = origin.position + posOffset + orientation.up * halfHeight;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1, point2);
    }

    // ============================================================================

    float base_radius;
    float base_height;

    void Awake()
    {
        base_radius = radius;
        base_height = height;
    }

    public void SetDefault()
    {
        radius = base_radius;
        height = base_height;
    }
}
