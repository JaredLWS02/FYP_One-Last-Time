using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransform : MonoBehaviour
{
    public bool randomizeOnAwake = true;

    [Header("Translate")]
    public bool randomTranslateX=false;
    public bool randomTranslateY=false, randomTranslateZ=false;
    public float minTranslate=-.1f, maxTranslate=.1f;
    Vector3 defaultPos;

    [Header("Rotate")]
    public bool randomRotateX=false;
    public bool randomRotateY=false, randomRotateZ=false;
    public float minRotate=-180, maxRotate=180;
    Vector3 defaultRot;

    [Header("Scale")]
    public bool randomScaleX=true;
    public bool randomScaleY=true, randomScaleZ=true;
    public float minScale=.9f, maxScale=1.1f;
    Vector3 defaultScale;

    // [Header("Mirror")]
    // public bool randomMirrorX=false;
    // public bool randomMirrorY=false, randomMirrorZ=false;

    // ============================================================================

    void Reset()
    {
        RecordDefaults();
    }

    void Awake()
    {
        RecordDefaults();

        if(!randomizeOnAwake) return;

        RandomTranslate();
        RandomRotate();
        RandomScale();
        //mirror();
    }

    // ============================================================================

    [ContextMenu("Record Defaults")]
    public void RecordDefaults()
    {
        defaultPos = transform.localPosition;
        defaultRot = transform.localEulerAngles;
        defaultScale = transform.localScale;
    }

    [ContextMenu("Reset Defaults")]
    public void ResetDefaults()
    {
        transform.localPosition = defaultPos;
        transform.localEulerAngles = defaultRot;
        transform.localScale = defaultScale;
    }

    // ============================================================================

    [ContextMenu("Random Translate")]
    public void RandomTranslate()
    {
        Vector3 pos = defaultPos;

        if(randomTranslateX)
        pos.x += Random.Range(minTranslate, maxTranslate);

        if(randomTranslateY)
        pos.y += Random.Range(minTranslate, maxTranslate);
        
        if(randomTranslateZ)
        pos.z += Random.Range(minTranslate, maxTranslate);

        transform.localPosition = pos;
    }

    [ContextMenu("Random Rotate")]
    public void RandomRotate()
    {
        Vector3 rot = defaultRot;

        if(randomRotateX)
        rot.x += Random.Range(minRotate, maxRotate);

        if(randomRotateY)
        rot.y += Random.Range(minRotate, maxRotate);
        
        if(randomRotateZ)
        rot.z += Random.Range(minRotate, maxRotate);

        transform.localEulerAngles = rot;
    }
    
    [ContextMenu("Random Scale")]
    public void RandomScale()
    {  
        Vector3 scale = defaultScale;

        float uniform_scale = Random.Range(minScale, maxScale);

        if(randomScaleX)
        scale.x *= uniform_scale;

        if(randomScaleY)
        scale.y *= uniform_scale;
        
        if(randomScaleZ)
        scale.z *= uniform_scale;

        transform.localScale = scale;
    }

    // ============================================================================
    
    // colliders dont support negative scale
    
    // void mirror()
    // {
    //     if(randomMirrorX && Random.Range(1,3)==1)
    //         transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);

    //     if(randomMirrorY && Random.Range(1,3)==1)
    //         transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y*-1, transform.localScale.z);
            
    //     if(randomMirrorZ && Random.Range(1,3)==1)
    //         transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z*-1);
    // }
}
