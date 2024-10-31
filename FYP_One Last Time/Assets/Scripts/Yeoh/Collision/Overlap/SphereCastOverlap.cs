using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastOverlap : OverlapScript
{
    public Transform orientation;

    public float radius=.5f;
    public float length=3;

    public override Collider[] GetOverlap()
    {
        Vector3 start = origin.position + posOffset;
        Vector3 direction = orientation.forward;

        RaycastHit[] hits = Physics.SphereCastAll(start, radius, direction, length, layers);

        Collider[] colliders = new Collider[hits.Length];

        for(int i=0; i<hits.Length; i++)
        {
            colliders[i] = hits[i].collider;
        }

        return colliders;
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

        Vector3 start = origin.position + posOffset;
        Vector3 end = start + orientation.forward * length;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
        Gizmos.DrawLine(start, end);
    }
}
