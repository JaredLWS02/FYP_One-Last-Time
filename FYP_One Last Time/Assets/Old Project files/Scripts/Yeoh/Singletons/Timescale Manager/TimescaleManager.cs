using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimescaleManager : MonoBehaviour
{
    public static TimescaleManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        Time.timeScale=1;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int tweenTimeLt=0;
    
    public void TweenTime(float to, float time=.01f)
    {
        if(to<0) to=0;

        LeanTween.cancel(tweenTimeLt);

        if(time>0)
        {
            tweenTimeLt = LeanTween.value(Time.timeScale, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{Time.timeScale=value;} )
                .id;
        }
        else Time.timeScale = to;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void HitStop(float fadeIn=.01f, float wait=.005f, float fadeOut=.25f)
    {
        if(Time.timeScale<1) return;

        if(hitStoppingRt!=null) StopCoroutine(hitStoppingRt);
        hitStoppingRt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
    }

    Coroutine hitStoppingRt;

    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);

        if(fadeIn>0) yield return new WaitForSecondsRealtime(fadeIn);
        if(wait>0) yield return new WaitForSecondsRealtime(wait);

        TweenTime(1, fadeOut);
    }
}
