using System.Collections;
using System.Collections.Generic;
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
    public bool animAlpha;
    public float inAlpha;
    public float outAlpha;
    float defAlpha;

    [Header("Autoplay")]
    public bool playOnEnable;
    public float playOnEnableAnimTime=.5f;
    public float playOnEnableDelay;

    void Awake()
    {
        defPos = transform.localPosition;
        defRot = transform.eulerAngles;
        defScale = transform.localScale;
        if(img) defAlpha = img.color.a;
    }

    void OnEnable()
    {
        if(playOnEnable)
        {
            if(enablingRt!=null) StopCoroutine(enablingRt);
            enablingRt = StartCoroutine(Enabling());
        }
    }
    void OnDisable()
    {
        Reset();
    }

    Coroutine enablingRt;

    IEnumerator Enabling()
    {
        yield return new WaitForSecondsRealtime(.001f); // give time for other awake methods to record default transforms

        Reset();

        yield return new WaitForSecondsRealtime(playOnEnableDelay);
        
        TweenIn(playOnEnableAnimTime);
    }

   void Start()
    {
        if(!playOnEnable) Reset(); // Must put after awake otherwise buttonanim records the zeroed transforms (starting transforms) as default, disappearing in mobile
    }

    public void Reset()
    {
        if(animPos) transform.localPosition = inPos;
        if(animRot) transform.eulerAngles = inRot;
        if(animScale) transform.localScale = inScale;
        if(animAlpha) TweenAlpha(inAlpha, 0);
    }

    public void TweenIn(float time)
    {
        Reset();

        if(time>0)
        {
            LeanTween.cancel(gameObject);

            if(animPos) LeanTween.moveLocal(gameObject, defPos, time).setEaseOutExpo().setIgnoreTimeScale(true);
            if(animRot) LeanTween.rotate(gameObject, defRot, time).setEaseInOutSine().setIgnoreTimeScale(true);
            if(animScale) LeanTween.scale(gameObject, defScale, time).setEaseOutCubic().setIgnoreTimeScale(true);
            if(animAlpha) TweenAlpha(defAlpha, time);

            //AudioManager.Current.PlaySFX(SFXManager.Current.sfxUITween, transform.position, false);
        }
        else
        {
            if(animPos) transform.localPosition = defPos;
            if(animRot) transform.eulerAngles = defRot;
            if(animScale) transform.localScale = defScale;
            if(animAlpha) TweenAlpha(defAlpha, 0);
        }
    }

    public void TweenOut(float time)
    {
        TweenIn(0);

        if(time>0)
        {
            LeanTween.cancel(gameObject);

            if(animPos) LeanTween.moveLocal(gameObject, outPos, time).setEaseInExpo().setIgnoreTimeScale(true).setOnComplete(Reset);
            if(animRot) LeanTween.rotate(gameObject, outRot, time).setEaseInOutSine().setIgnoreTimeScale(true).setOnComplete(Reset);
            if(animScale) LeanTween.scale(gameObject, outScale, time).setEaseInCubic().setIgnoreTimeScale(true).setOnComplete(Reset);
            if(animAlpha) TweenAlpha2(outAlpha, time);

            //AudioManager.Current.PlaySFX(SFXManager.Current.sfxUITween, transform.position, false);
        }
        else
        {
            if(animPos) transform.localPosition = outPos;
            if(animRot) transform.eulerAngles = outRot;
            if(animScale) transform.localScale = outScale;
            if(animAlpha) TweenAlpha(outAlpha, 0);
        }
    }

    int tweenAlphaLt=0;
    public void TweenAlpha(float to, float time)
    {
        if(!img) return;

        LeanTween.cancel(tweenAlphaLt);

        if(time>0)
        {
            tweenAlphaLt = LeanTween.value(img.color.a, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{img.color = new Color(img.color.r, img.color.g, img.color.b, value);} )
                .id;
        }
        else img.color = new Color(img.color.r, img.color.g, img.color.b, to);
    }

    public void TweenAlpha2(float to, float time)
    {
        if(!img) return;

        LeanTween.cancel(tweenAlphaLt);

        if(time>0)
        {
            tweenAlphaLt = LeanTween.value(img.color.a, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{img.color = new Color(img.color.r, img.color.g, img.color.b, value);} )
                .setOnComplete(Reset)
                .id;
        }
        else img.color = new Color(img.color.r, img.color.g, img.color.b, to);
    }

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
