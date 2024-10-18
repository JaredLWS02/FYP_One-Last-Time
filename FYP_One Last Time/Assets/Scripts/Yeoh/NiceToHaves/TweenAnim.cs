using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class TweenAnim : MonoBehaviour
{
    [Header("Local Position")]
    public bool animPos;
    public Vector3 inPos;
    public Vector3 outPos;
    Vector3 defPos;

    [Header("Rotation")]
    public bool animRot;
    public Vector3 inRot;
    public Vector3 outRot;
    Vector3 defRot;

    [Header("Scale")]
    public bool animScale;
    public Vector3 inScale;
    public Vector3 outScale;
    Vector3 defScale;

    [Header("Alpha")]
    public Image img;
    public SpriteRenderer sr;
    public bool animAlpha;
    public float inAlpha;
    public float outAlpha;
    float defAlpha;

    [Header("Autoplay")]
    public bool playOnEnable;
    public float playOnEnableAnimTime=.5f;
    public float playOnEnableDelay;

    // ============================================================================

    void Awake()
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;

        defPos = transform.localPosition;
        defRot = transform.eulerAngles;
        defScale = transform.localScale;
        if(img) defAlpha = img.color.a;
    }

    void OnEnable()
    {
        if(playOnEnable)
        {
            if(enabling_crt!=null) StopCoroutine(enabling_crt);
            enabling_crt = StartCoroutine(Enabling());
        }
    }
    void OnDisable()
    {
        Reset();
    }

    // ============================================================================

    Coroutine enabling_crt;
    IEnumerator Enabling()
    {
        // give time for other awake methods to record default transforms
        yield return new WaitForSecondsRealtime(.001f);

        Reset();

        yield return new WaitForSecondsRealtime(playOnEnableDelay);
        
        TweenIn(playOnEnableAnimTime);
    }

   void Start()
    {
        // Must put after awake otherwise buttonanim records the zeroed transforms (starting transforms) as default, disappearing in mobile
        if(!playOnEnable) Reset(); 
    }

    // ============================================================================

    Tween posTween;
    Tween rotTween;
    Tween scaleTween;
    Tween alphaTween;

    public void Reset()
    {
        if(animPos)
        {
            posTween.Stop();
            transform.localPosition = inPos;
        }
        if(animRot)
        {
            rotTween.Stop();
            transform.eulerAngles = inRot;
        }
        if(animScale)
        {
            scaleTween.Stop();
            transform.localScale = inScale;
        }
        if(animAlpha)
        {
            TweenAlpha(inAlpha, 0);
        }
    }

    // ============================================================================

    public void TweenIn(float time)
    {
        Reset();

        if(animPos)
        {
            posTween.Stop();
            if(time>0) posTween = Tween.LocalPosition(transform, defPos, time, Ease.InOutSine, useUnscaledTime: true);
            else transform.localPosition = defPos;
        }
        if(animRot)
        {
            rotTween.Stop();
            if(time>0) rotTween = Tween.Rotation(transform, Quaternion.Euler(defRot), time, Ease.InOutSine, useUnscaledTime: true);
            else transform.rotation = Quaternion.Euler(defRot);
        }
        if(animScale)
        {
            scaleTween.Stop();
            if(time>0) scaleTween = Tween.Scale(transform, defScale, time, Ease.OutCubic, useUnscaledTime: true);
            else transform.localScale = defScale;
        }
        if(animAlpha)
        {
            TweenAlpha(defAlpha, time);
        }

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxUITween, transform.position, false);
    }

    public void TweenOut(float time)
    {
        TweenIn(0);

        if(animPos)
        {
            posTween.Stop();
            if(time>0) posTween = Tween.LocalPosition(transform, outPos, time, Ease.InExpo, useUnscaledTime: true).OnComplete(Reset);
            else transform.localPosition = outPos;
        }
        if(animRot)
        {
            rotTween.Stop();
            if(time>0) rotTween = Tween.Rotation(transform, Quaternion.Euler(outRot), time, Ease.InOutSine, useUnscaledTime: true).OnComplete(Reset);
            else transform.rotation = Quaternion.Euler(outRot);
        }
        if(animScale)
        {
            scaleTween.Stop();
            if(time>0) scaleTween = Tween.Scale(transform, outScale, time, Ease.InCubic, useUnscaledTime: true).OnComplete(Reset);
            else transform.localScale = outScale;
        }
        if(animAlpha)
        {
            TweenAlpha(outAlpha, time);
        }

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxUITween, transform.position, false);
    }

    // ============================================================================

    void TweenAlpha(float to, float time)
    {
        alphaTween.Stop();

        if(sr)
        {
            if(time>0) alphaTween = Tween.Alpha(sr, to, time, Ease.InOutSine, useUnscaledTime: true);
            else
            {
                Color color = sr.color;
                color.a = to;
                sr.color = color;
            }
        }
        if(img)
        {
            if(time>0) alphaTween = Tween.Alpha(img, to, time, Ease.InOutSine, useUnscaledTime: true);
            else
            {
                Color color = img.color;
                color.a = to;
                img.color = color;
            }
        }
    }

    // ============================================================================

    //[Button] // requires Odin Inspector??
    [ContextMenu("Record Local Position")]
    void RecordCurrentPosition()
    {
        inPos=transform.localPosition;
        outPos=transform.localPosition;
    }
    [ContextMenu("Record Rotation")]
    void RecordCurrentRotation()
    {
        inRot=transform.eulerAngles;
        outRot=transform.eulerAngles;
    }
    [ContextMenu("Record Scale")]
    void RecordCurrentScale()
    {
        inScale=transform.localScale;
        outScale=transform.localScale;
    }
    [ContextMenu("Record Alpha")]
    void RecordCurrenAlpha()
    {
        if(!img) return;
        inAlpha=img.color.a;
        outAlpha=img.color.a;
    }
    
}
