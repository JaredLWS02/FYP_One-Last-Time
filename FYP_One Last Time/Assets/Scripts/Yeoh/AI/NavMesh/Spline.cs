using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public Transform start;
    public Transform middle;
    public Transform end;

    [ContextMenu("Create Spline Points")]
    void CreatePoints()
    {
        if(start) DestroyImmediate(start.gameObject);
        if(middle) DestroyImmediate(middle.gameObject);
        if(end) DestroyImmediate(end.gameObject);

        GameObject start_obj = new("Start");
        GameObject mid_obj = new("Middle");
        GameObject end_obj = new("End");

        start = start_obj.transform;
        middle = mid_obj.transform;
        end = end_obj.transform;

        start_obj.transform.parent = transform;
        mid_obj.transform.parent = transform;
        end_obj.transform.parent = transform;
    }

    // ============================================================================
    
    public void SetSpline(Vector3 start, Vector3 middle, Vector3 end)
    {
        // all transforms shouldn't be null
        if(!this.start || !this.middle || !this.end) return;

        this.start.position = start;
        this.middle.position = middle;
        this.end.position = end;
    }

    // ============================================================================

    public Vector3 CalcPos(float lerp01, Vector3 start_pos, Vector3 mid_pos, Vector3 end_pos)
    {
        lerp01 = Mathf.Clamp01(lerp01);

        Vector3 start_to_middle = Vector3.Lerp(start_pos, mid_pos, lerp01);
        Vector3 middle_to_end = Vector3.Lerp(mid_pos, end_pos, lerp01);

        return Vector3.Lerp(start_to_middle, middle_to_end, lerp01);
    }

    public Vector3 CalcPos(float lerp01)
    {
        return CalcPos(lerp01, start.position, middle.position, end.position);
    }

    public Vector3 CalcPosFromStart(float lerp01, Vector3 start_pos)
    {
        return CalcPos(lerp01, start_pos, middle.position, end.position);
    }

    public Vector3 CalcPosFromEnd(float lerp01, Vector3 end_pos)
    {
        return CalcPos(lerp01, start.position, middle.position, end_pos);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos=true;
    public bool showPoints;
    public int granularity=10;
    public Color splineColor = Color.magenta;

    void OnDrawGizmos()
    {
        if(!showGizmos) return;
        // all transforms shouldn't be null
        if(!start || !middle || !end) return;

        Gizmos.color = splineColor;
        DrawSplinePoints();
        DrawSplineLine();
    }

    void DrawSplinePoints(float sphere_size=.1f)
    {
        if(!showPoints) return;
        Gizmos.DrawSphere(start.position, sphere_size);
        Gizmos.DrawSphere(middle.position, sphere_size);
        Gizmos.DrawSphere(end.position, sphere_size);
    }

    void DrawSplineLine()
    {
        for(int i=0; i<granularity; i++)
        {
            Vector3 start_point = i==0 ? start.position : CalcPos(i / (float)granularity);

            Vector3 end_point = i==granularity-1 ? end.position : CalcPos((i+1) / (float)granularity);

            Gizmos.DrawLine(start_point, end_point);
        }
    }

}

// Tutorial by SunnyValleyStudio YouTube