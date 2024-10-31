using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOverlap : OverlapScript
{
    public float range=.5f;

    public override Collider[] GetOverlap()
    {
        return Physics.OverlapSphere(origin.position + posOffset, range, layers);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos=true;
    public Color gizmoColor = new(1, 1, 1, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!origin) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(origin.position + posOffset, range);
    }
}
