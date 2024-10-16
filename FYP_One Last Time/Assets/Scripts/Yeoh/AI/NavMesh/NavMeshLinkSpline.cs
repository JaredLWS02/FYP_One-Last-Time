using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshLink))]
[RequireComponent(typeof(Spline))]

[ExecuteInEditMode]

public class NavMeshLinkSpline : MonoBehaviour
{
    NavMeshLink navMeshLink;
    Spline spline;

    void Awake()
    {
        navMeshLink = GetComponent<NavMeshLink>();
        spline = GetComponent<Spline>();
    }
    
    void OnValidate()
    {
        navMeshLink = GetComponent<NavMeshLink>();
        spline = GetComponent<Spline>();
    }

    // ============================================================================

    public float heightOffset=4;
    [Range(.01f, .99f)]
    public float arcOffset=.5f;

#if UNITY_EDITOR

    void Update()
    {
        if(!spline || !navMeshLink) return;

        Vector3 start_pos = transform.TransformPoint(navMeshLink.startPoint);
        Vector3 end_pos = transform.TransformPoint(navMeshLink.endPoint);

        Vector3 mid_pos = Vector3.Lerp(start_pos, end_pos, arcOffset);

        mid_pos.y += heightOffset;

        spline.SetSpline(start_pos, mid_pos, end_pos);
    }

#endif

}

// Tutorial by SunnyValleyStudio YouTube