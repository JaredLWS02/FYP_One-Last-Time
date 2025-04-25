using UnityEngine;

[ExecuteAlways]
public class BoxOverlapGizmo : MonoBehaviour
{
    public BoxOverlap box;

    public Color gizmoColor = new(1,0,0, 0.25f);

    void OnDrawGizmos()
    {
        if(!box) return;

        Gizmos.color = gizmoColor;

        Gizmos.matrix = box.transform.localToWorldMatrix;
        Gizmos.DrawCube(box.posOffset, box.boxSize);
    }
}
