using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LockTransform : MonoBehaviour
{
    public bool enableAllLocks=true;
    public bool fixedUpdate=true;

    [Header("Position Lock")]
    public Vector3Int lockPosAxis;
    public Transform lockPosTo;
    public Vector3 localPos;

    [Header("Rotation Lock")]
    public Vector3Int lockAngleAxis;
    public Vector3 lockAngleTo;

    // cant work because only got localScale, not worldScale
    //public Vector3Int lockScale;
    //public Vector3 scaleOffset = Vector3.one;

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
        if(!Application.isPlaying) Lock();
    }
#endif

    void Update()
    {
        if(Application.isPlaying && !fixedUpdate) Lock();
    }

    void FixedUpdate()
    {
        if(Application.isPlaying && fixedUpdate) Lock();
    }

    void Lock()
    {
        if(!enableAllLocks) return;

        if(lockPosTo)
        transform.position = new
        (
            lockPosAxis.x==0 ? transform.position.x : lockPosTo.position.x + localPos.x,
            lockPosAxis.y==0 ? transform.position.y : lockPosTo.position.y + localPos.y,
            lockPosAxis.z==0 ? transform.position.z : lockPosTo.position.z + localPos.z
        );
        
        transform.rotation = Quaternion.Euler
        (
            lockAngleAxis.x==0 ? transform.eulerAngles.x : lockAngleTo.x,
            lockAngleAxis.y==0 ? transform.eulerAngles.y : lockAngleTo.y,
            lockAngleAxis.z==0 ? transform.eulerAngles.z : lockAngleTo.z
        );
        
        // transform.localScale = new
        // (
        //     lockScale.x==0 ? transform.localScale.x : scaleOffset.x,
        //     lockScale.y==0 ? transform.localScale.y : scaleOffset.y,
        //     lockScale.z==0 ? transform.localScale.z : scaleOffset.z
        // );
    }

    [ContextMenu("Record Local Position")]
    void RecordLocalPos()
    {
        localPos = transform.localPosition;
    }
    [ContextMenu("Record Current Rotation")]
    void RecordRCurrentRotation()
    {
        lockAngleTo = transform.localRotation.eulerAngles;
    }
    // [ContextMenu("Record Scale Offset")]
    // void RecordScaleOffset()
    // {
    //     scaleOffset = transform.localScale;
    // }
}
