using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDoughnut2D : MonoBehaviour
{
    public Transform origin;

    public Vector2 rangeMinMax = new(5,10);

    public Vector2 GetRandomPos(Vector3 origin_pos)
    {
        float angle = Random.Range(0, Mathf.PI*2);
        
        float distance = Mathf.Sqrt(Random.Range(rangeMinMax.x * rangeMinMax.x, rangeMinMax.y * rangeMinMax.y));

        float x = distance * Mathf.Cos(angle);
        float y = distance * Mathf.Sin(angle);

        return origin_pos + new Vector3(x, y);
    }

    public Vector2 GetRandomPos()
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
