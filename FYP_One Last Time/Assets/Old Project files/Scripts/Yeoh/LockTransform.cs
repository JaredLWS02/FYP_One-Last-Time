using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LockTransform : MonoBehaviour
{
    public bool enableLock=true;
    public bool fixedUpdate=true;

    [Header("Axis")]
    public Transform constrainPos;
    public Vector3 lockPosition;
    public Vector3 lockRotation;

    [Header("Offsets")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

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
        if(!enableLock) return;

        if(constrainPos)
        transform.position =  new Vector3
        (
            lockPosition.x==0 ? transform.position.x : constrainPos.position.x + positionOffset.x,
            lockPosition.y==0 ? transform.position.y : constrainPos.position.y + positionOffset.y,
            lockPosition.z==0 ? transform.position.z : constrainPos.position.z + positionOffset.z
        );
        
        transform.rotation = Quaternion.Euler
        (
            lockRotation.x==0 ? transform.eulerAngles.x : 0 + rotationOffset.x,
            lockRotation.y==0 ? transform.eulerAngles.y : 0 + rotationOffset.y,
            lockRotation.z==0 ? transform.eulerAngles.z : 0 + rotationOffset.z
        );
    }

    [ContextMenu("Record Position Offset")]
    void RecordPosOffset()
    {
        positionOffset = transform.localPosition;
    }
}
