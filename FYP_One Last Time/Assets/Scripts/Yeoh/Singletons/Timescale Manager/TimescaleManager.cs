using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimescaleManager : MonoBehaviour
{
    public static TimescaleManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
        
    // ============================================================================

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    void OnSceneUnloaded(Scene scene)
    {
        TweenTime(1, 0);
    }
    
    // ============================================================================

    Tween timeTween;
    
    public void TweenTime(float to, float time=.2f)
    {
        if(to<0) to=0;

        timeTween.Stop();
        if(time>0) timeTween = Tween.GlobalTimeScale(to, time, Ease.InOutSine);
        else Time.timeScale = to;
    }

    // ============================================================================

    public float hitstopTimescale = 0.3f;

    public void HitStop(float fadeIn=.1f, float wait=.01f, float fadeOut=.25f)
    {
        if(Time.timeScale<1) return;

        CancelHitStop();
        hitStopping_crt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
    }

    Coroutine hitStopping_crt;

    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(hitstopTimescale, fadeIn);

        if(fadeIn>0) yield return new WaitForSecondsRealtime(fadeIn);
        if(wait>0) yield return new WaitForSecondsRealtime(wait);

        TweenTime(1, fadeOut);
    }

    void CancelHitStop()
    {
        if(hitStopping_crt!=null) StopCoroutine(hitStopping_crt);
        TweenTime(1, 0);
    }

    // ============================================================================

    public float pauseFadeTime = 0.1f;

    public void Pause(bool toggle)
    {
        CancelHitStop();

        TweenTime(toggle ? 0 : 1, pauseFadeTime);
    }
}
