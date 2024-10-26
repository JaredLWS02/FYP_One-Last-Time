using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDoughnut : MonoBehaviour
{
    public Transform origin;

    public Vector2 rangeMinMax = new(5,10);

    public Vector3 GetRandomPos(Vector3 origin_pos)
    {
        float random_range = Random.Range(rangeMinMax.x, rangeMinMax.y);

        Vector3 random_dir = Random.insideUnitSphere.normalized;

        Vector3 offset_pos = random_range * random_dir;

        return origin_pos + offset_pos;
    }
    
    public Vector3 GetRandomPos()
    {
        return GetRandomPos(origin.position);
    }
    
    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = Color.cyan;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Vector3 pos = origin ? origin.position : Vector3.zero;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(pos, rangeMinMax.x);
        Gizmos.DrawWireSphere(pos, rangeMinMax.y);
    }
}
