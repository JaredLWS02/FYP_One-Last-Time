using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]

public class SpriteBillboardFlip : MonoBehaviour
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
        
        Flip();
    }

#endif

    // ============================================================================

    public bool fixedUpdate;

    void Update()
    {
        if(!Application.isPlaying) return;

        if(!fixedUpdate) Flip();
    }

    void FixedUpdate()
    {
        if(!Application.isPlaying) return;
        
        if(fixedUpdate) Flip();
    }

    // ============================================================================

    [Space]
    public Transform orientation;
    public SpriteRenderer sr;

    [Space]
    public bool flipX = true;
    public Vector3 axisFacingAwayCam = new(-1,0,0);

    [Space]
    public bool flipY;
    public Vector3 axisUpWithCam = new(0,1,0);

    void Flip()
    {
        if(!orientation) return;
        if(!sr) return;
        if(!Camera.main) return;

        Transform cam_tr = Camera.main.transform;
        if(!cam_tr) return;
        
        Vector3 dir_away = orientation.TransformDirection(axisFacingAwayCam);
        Vector3 dir_up = orientation.TransformDirection(axisUpWithCam);

        float dot_x = Vector3.Dot(cam_tr.forward, dir_away);
        float dot_y = Vector3.Dot(cam_tr.up, dir_up);

        bool is_facing_away_cam = dot_x >= 0;
        bool is_up_with_cam = dot_y >= 0;

        if(flipX)
        {
            if(flipY)
                sr.flipX = (!is_facing_away_cam && is_up_with_cam) || (is_facing_away_cam && !is_up_with_cam);
            else
                sr.flipX = !is_facing_away_cam;
        }
        
        if(flipY) sr.flipY = !is_up_with_cam;
    }
}
