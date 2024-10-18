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
    
    public void TweenTime(float to, float time=.01f)
    {
        if(to<0) to=0;

        timeTween.Stop();
        if(time>0) timeTween = Tween.GlobalTimeScale(to, time, Ease.InOutSine);
        else Time.timeScale = to;
    }

    // ============================================================================

    public void HitStop(float fadeIn=.01f, float wait=.005f, float fadeOut=.25f)
    {
        if(Time.timeScale<1) return;

        if(hitStopping_crt!=null) StopCoroutine(hitStopping_crt);
        hitStopping_crt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
    }

    Coroutine hitStopping_crt;

    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);

        if(fadeIn>0) yield return new WaitForSecondsRealtime(fadeIn);
        if(wait>0) yield return new WaitForSecondsRealtime(wait);

        TweenTime(1, fadeOut);
    }
}
