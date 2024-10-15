using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleTransform : MonoBehaviour
{
    Vector3 seed;

    [Header("Position")]
    public bool wigglePos;
    public Vector3 posFrequency;
    public Vector3 posMagnitude;
    Vector3 defPos;

    [Header("Rotation")]
    public bool wiggleRot;
    public Vector3 rotFrequency;
    public Vector3 rotMagnitude;
    Vector3 defRot;

    [Header("Scale 1")]
    public bool wiggleScale1;
    public float scaleFrequency;
    public float scaleMagnitude;
    Vector3 defScale;

    [Header("Scale 2")]
    public bool wiggleScale2;
    public Vector3 scaleFrequency2;
    public Vector3 scaleMagnitude2;

    [Header("Time")]
    public bool ignoreTime;
    float time;
    
    void Awake()
    {
        seed = new Vector3(Random.value*999, Random.value*999, Random.value*999);

        defPos=transform.localPosition;
        defRot=transform.localEulerAngles;
        defScale=transform.localScale;
    }

    void Update()
    {
        if(ignoreTime)
        {
            time=Time.unscaledTime;
        }
        else time=Time.time;

        WigglePos();
        WiggleRot();
        WiggleScale1();
        WiggleScale2();
    }

    public float Wiggle(float seed, float freq, float mag, float offset=0)
    {
        if(freq!=0 && mag!=0)
        {
            return (Mathf.PerlinNoise(seed, time * freq)*2-1) * mag + offset;
        }
        return offset;
    }

    void WigglePos()
    {
        if(!wigglePos) return;

        if(posFrequency==Vector3.zero || posMagnitude==Vector3.zero) return;
        
        Vector3 wiggle = new Vector3
        (
            Wiggle(seed.x, posFrequency.x, posMagnitude.x),
            Wiggle(seed.y, posFrequency.y, posMagnitude.y),
            Wiggle(seed.z, posFrequency.z, posMagnitude.z)
        );

        transform.localPosition = defPos + wiggle;
    }

    void WiggleRot()
    {
        if(!wiggleRot) return;
        
        if(rotFrequency==Vector3.zero || rotMagnitude==Vector3.zero) return;
        
        Vector3 wiggle = new Vector3
        (
            Wiggle(seed.x, rotFrequency.x, rotMagnitude.x),
            Wiggle(seed.y, rotFrequency.y, rotMagnitude.y),
            Wiggle(seed.z, rotFrequency.z, rotMagnitude.z)
        );

        transform.localEulerAngles = defRot + wiggle;
    }

    void WiggleScale1()
    {
        if(!wiggleScale1) return;
        
        if(scaleFrequency==0 || scaleMagnitude==0) return;
        
        Vector3 wiggle = new Vector3
        (
            Wiggle(seed.x, scaleFrequency, scaleMagnitude, scaleMagnitude),
            Wiggle(seed.x, scaleFrequency, scaleMagnitude, scaleMagnitude),
            Wiggle(seed.x, scaleFrequency, scaleMagnitude, scaleMagnitude)
        );

        transform.localScale = defScale + wiggle;
    }
    
    void WiggleScale2()
    {
        if(!wiggleScale2) return;
        
        if(scaleFrequency2==Vector3.zero || scaleMagnitude2==Vector3.zero) return;
        
        Vector3 wiggle = new Vector3
        (
            Wiggle(seed.x, scaleFrequency2.x, scaleMagnitude2.x),
            Wiggle(seed.y, scaleFrequency2.y, scaleMagnitude2.y),
            Wiggle(seed.z, scaleFrequency2.z, scaleMagnitude2.z)
        );

        transform.localScale = defScale + wiggle;
    }

    public void ShakePos(float time)
    {
        if(shakingPosRt!=null) StopCoroutine(shakingPosRt);
        shakingPosRt = StartCoroutine(ShakingPos(time));
    }
    Coroutine shakingPosRt;
    IEnumerator ShakingPos(float time)
    {
        wigglePos=true;
        yield return new WaitForSeconds(time);
        wigglePos=false;
        ResetPos();
    }

    public void ShakeRot(float time)
    {
        if(shakingRotRt!=null) StopCoroutine(shakingRotRt);
        shakingRotRt = StartCoroutine(ShakingRot(time));
    }
    Coroutine shakingRotRt;
    IEnumerator ShakingRot(float time)
    {
        wiggleRot=true;
        yield return new WaitForSeconds(time);
        wiggleRot=false;
        ResetRot();
    }

    public void ShakeScale(float time)
    {
        if(shakingScaleRt!=null) StopCoroutine(shakingScaleRt);
        shakingScaleRt = StartCoroutine(ShakingScale(time));
    }
    Coroutine shakingScaleRt;
    IEnumerator ShakingScale(float time)
    {
        wiggleScale1=true;
        yield return new WaitForSeconds(time);
        wiggleScale1=false;
        ResetScale();
    }
    
    public void ShakeScale2(float time)
    {
        if(shakingScaleRt2!=null) StopCoroutine(shakingScaleRt2);
        shakingScaleRt2 = StartCoroutine(ShakingScale2(time));
    }
    Coroutine shakingScaleRt2;
    IEnumerator ShakingScale2(float time)
    {
        wiggleScale2=true;
        yield return new WaitForSeconds(time);
        wiggleScale2=false;
        ResetScale();
    }
    
    [ContextMenu("Reset Position")]
    public void ResetPos()
    {
        transform.localPosition = defPos;
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
