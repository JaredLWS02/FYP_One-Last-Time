using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]

public class SpriteBillboard : MonoBehaviour
{

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
        if(!editorUpdate) return;
        if(Application.isPlaying) return;
        
        Billboard();
    }

#endif

    // ============================================================================

    public bool enableBillboard=true;
    public bool fixedUpdate=true;
    public bool editorUpdate=true;
    public Vector3 rotateAxis = Vector3.one;

    void Update()
    {
        if(!Application.isPlaying) return;

        if(!fixedUpdate) Billboard();
    }

    void FixedUpdate()
    {
        if(!Application.isPlaying) return;
        
        if(fixedUpdate) Billboard();
    }

    void Billboard()
    {
        if(!enableBillboard) return;
        if(!Camera.main) return;

        Vector3 camera_angles = Camera.main.transform.eulerAngles;
        Vector3 target_angles = Vector3.zero;

        if(rotateAxis.x > 0) target_angles.x = camera_angles.x;
        if(rotateAxis.y > 0) target_angles.y = camera_angles.y;
        if(rotateAxis.z > 0) target_angles.z = camera_angles.z;

        transform.rotation = Quaternion.Euler(target_angles);

        Vector3 local_angles = transform.localEulerAngles;

        if(rotateAxis.x <= 0) local_angles.x = 0;
        if(rotateAxis.y <= 0) local_angles.y = 0;
        if(rotateAxis.z <= 0) local_angles.z = 0;

        transform.localRotation = Quaternion.Euler(local_angles);
    }
}
