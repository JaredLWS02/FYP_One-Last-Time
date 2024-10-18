using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Vector3 boxSize = new(.5f, .05f, .5f);
    public Vector3 boxOffset = Vector3.zero;

    public LayerMask groundLayer;

    public bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + boxOffset, boxSize, transform.rotation, groundLayer);

        foreach(var coll in colliders)
        {
            if(!coll.isTrigger) return true;
        }
        return false;
    }

    // ============================================================================

    [Header("Debug")]
    public Color gizmoColor = Color.blue;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        Vector3 boxCenter = transform.position + boxOffset;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // Reset the Gizmos matrix to default
        Gizmos.matrix = Matrix4x4.identity;
    }
}
