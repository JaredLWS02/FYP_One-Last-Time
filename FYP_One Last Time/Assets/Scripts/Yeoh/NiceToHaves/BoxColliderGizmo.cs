using UnityEngine;

[ExecuteAlways]
public class BoxColliderGizmo : MonoBehaviour
{
    public BoxCollider box;

    public Color gizmoColor = new(1,1,0, 0.25f);

    void OnDrawGizmos()
    {
        if(!box) return;

        Gizmos.color = gizmoColor;

        Gizmos.matrix = box.transform.localToWorldMatrix;
        Gizmos.DrawCube(box.bounds.center - box.transform.position, box.bounds.size);
    }
}
