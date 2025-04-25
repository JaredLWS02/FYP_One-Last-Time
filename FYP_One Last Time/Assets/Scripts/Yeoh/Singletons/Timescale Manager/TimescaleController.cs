using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimescaleController : MonoBehaviour
{
    TimescaleManager TimeM;

    void OnEnable()
    {
        TimeM = TimescaleManager.Current;

        events.OnEnable?.Invoke();
    }

    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    public float fadeTime=.2f;

    public void SetFadeTime(float to) => fadeTime=to;

    public void TweenTime(float to) => TimeM.TweenTime(to, fadeTime);

    // ============================================================================
    
    [Header("HitStop")]
    public float hitstopIn_ = .1f;
    public float hitstopWait_ = .01f;
    public float hitstopOut = .25f;

    public void SetHitstopIn(float to) => hitstopIn_=to;
    public void SetHitstopWait(float to) => hitstopWait_=to;
    public void SetHitstopOut(float to) => hitstopOut=to;

    public void HitStop() => TimeM.HitStop(hitstopIn_, hitstopWait_, hitstopOut);
    
    // ============================================================================

    public void Pause(bool toggle) => TimeM.Pause(toggle);

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;
}
