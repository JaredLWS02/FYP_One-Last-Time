using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;

    // ============================================================================

    [System.Serializable]
    public struct Optional
    {
        public Transform startTr;
        public Transform endTr;

        public BaseRaycast ray;
    }

    public Optional optional;

    // ============================================================================

    void Start()
    {
        if(lineRenderer)
        lineRenderer.positionCount = 2;
    }

    void Reset()
    {
        if(lineRenderer)
        lineRenderer.positionCount = 2;
    }

    // ============================================================================

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }
    void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    void EditorUpdate()
    {
        if(Application.isPlaying) return;
        SetLine();
    }
#endif

    // ============================================================================

    void Update()
    {
        if(!Application.isPlaying) return;
        SetLine();
    }

    // ============================================================================

    Vector3 startPoint;
    Vector3 endPoint;
    
    void SetLine()
    {
        if(!lineRenderer) return;

        if(optional.startTr) startPoint = optional.startTr.position;
        if(optional.endTr) endPoint = optional.endTr.position;

        if(optional.ray)
        {
            startPoint = optional.ray.GetStartPoint();
            endPoint = optional.ray.GetHitEndPoint();
        }

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

}
