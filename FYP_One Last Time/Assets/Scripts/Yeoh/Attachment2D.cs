using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Attachment2D : MonoBehaviour
{
    public List<Transform> followers = new();

    [Header("Invert")]
    public SideFlip flip;
    public Vector3Int invertPosAxis = new(1,0,0);
    public bool localPos=true;
    public Vector3Int invertRotAxis = new(0,0,1);
    public bool localRot=true;

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
        if(!Application.isPlaying) UpdateFollow();
    }
#endif

    // ============================================================================

    void Update()
    {
        if(Application.isPlaying) UpdateFollow();
    }

    // ============================================================================

    void UpdateFollow()
    {
        foreach(var follower in followers)
        {
            if(localPos)
            {
                follower.localPosition = flip.faceR ?
                    transform.localPosition :
                    Mirror(transform.localPosition, invertPosAxis);
            }
            else
            {
                follower.position = flip.faceR ?
                    transform.position :
                    Mirror(transform.position, invertPosAxis);
            }
            
            if(localRot)
            {
                follower.localEulerAngles = flip.faceR ?
                    transform.localEulerAngles :
                    Mirror(transform.localEulerAngles, invertRotAxis);
            }
            else
            {
                follower.eulerAngles = flip.faceR ?
                    transform.eulerAngles :
                    Mirror(transform.eulerAngles, invertRotAxis);
            }
        }
    }

    // ============================================================================

    Vector3 Mirror(Vector3 vector, Vector3Int axis)
    {
        return new
        (
            axis.x>0 ? -vector.x : vector.x,
            axis.y>0 ? -vector.y : vector.y,
            axis.z>0 ? -vector.z : vector.z
        );
    }
}
