using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineTransform : MonoBehaviour
{
    public Transform relativeTo;

    [Header("Position")]
    public bool sinePos;
    public Vector3 posFrequency;
    public Vector3 posMagnitude;
    public bool localPos=true;
    Vector3 defPos;

    [Header("Rotation")]
    public bool sineRot;
    public Vector3 rotFrequency;
    public Vector3 rotMagnitude;
    Vector3 defRot;

    [Header("Scale")]
    public bool sineScale;
    public float scaleFrequency;
    public float scaleMagnitude;
    Vector3 defScale;

    [Header("Time")]
    public bool ignoreTime;
    float time;
    
    void Awake()
    {
        defPos = transform.localPosition;
        defRot = transform.localEulerAngles;
        defScale = transform.localScale;
    }

    void Update()
    {
        if(ignoreTime)
        {
            time=Time.unscaledTime;
        }
        else time=Time.time;

        SinePos();
        SineRot();
        SineScale();
    }

    public float Sine(float freq, float mag, float offset=0)
    {
        if(freq!=0 && mag!=0)
        {
            return Mathf.Sin(time * freq) * mag + offset;
        }
        return offset;
    }

    void SinePos()
    {
        if(!sinePos) return;

        if(posFrequency==Vector3.zero || posMagnitude==Vector3.zero) return;
        
        Vector3 sine = new Vector3
        (
            Sine(posFrequency.x, posMagnitude.x),
            Sine(posFrequency.y, posMagnitude.y),
            Sine(posFrequency.z, posMagnitude.z)
        );

        if(localPos) transform.localPosition = defPos + sine;
        else transform.position = relativeTo.position + sine;
    }

    void SineRot()
    {
        if(!sineRot) return;

        if(rotFrequency==Vector3.zero || rotMagnitude==Vector3.zero) return;
        
        Vector3 sin = new Vector3
        (
            Sine(rotFrequency.x, rotMagnitude.x),
            Sine(rotFrequency.y, rotMagnitude.y),
            Sine(rotFrequency.z, rotMagnitude.z)
        );

        transform.localEulerAngles = defRot + sin;
    }

    void SineScale()
    {
        if(!sineScale) return;

        if(scaleFrequency==0 || scaleMagnitude==0) return;
        
        Vector3 sin = new Vector3
        (
            Sine(scaleFrequency, scaleMagnitude, scaleMagnitude),
            Sine(scaleFrequency, scaleMagnitude, scaleMagnitude),
            Sine(scaleFrequency, scaleMagnitude, scaleMagnitude)
        );

        transform.localScale = defScale + sin;
    }

    [ContextMenu("Reset Position")]
    public void ResetPos()
    {
        if(localPos) transform.localPosition = defPos;
        else transform.position = relativeTo.position;
    }
    
    [ContextMenu("Reset Rotation")]
    public void ResetRot()
    {
        transform.localEulerAngles = defRot;
    }
    
    [ContextMenu("Reset Scale")]
    public void ResetScale()
    {
        transform.localScale = defScale;
    }
}
