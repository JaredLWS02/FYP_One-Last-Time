using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOverlap : BaseOverlap
{
    [Header("Box Overlap")]
    public Vector3 boxSize = Vector3.one;

    public override Collider[] GetOverlap()
    {
        return Physics.OverlapBox(origin.position + posOffset, boxSize*.5f, origin.rotation, layers);
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

        Vector3 boxCenter = origin.position + posOffset;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, origin.rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // Reset the Gizmos matrix to default
        Gizmos.matrix = Matrix4x4.identity;
    }

    // ============================================================================

    Vector3 base_size;

    void Awake()
    {
        base_size = boxSize;
    }

    public void SetDefault()
    {
        boxSize = base_size;
    }
}
