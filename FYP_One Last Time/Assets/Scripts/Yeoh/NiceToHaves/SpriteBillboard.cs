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
        if(Application.isPlaying) return;
        
        Billboard();
    }

#endif

    // ============================================================================

    public bool enableBillboard=true;
    public bool fixedUpdate=true;
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

        Vector3 camera_angles = Camera.main.transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(
            rotateAxis.x>0 ? camera_angles.x : 0,
            rotateAxis.y>0 ? camera_angles.y : 0,
            rotateAxis.z>0 ? camera_angles.z : 0);
    }
}
