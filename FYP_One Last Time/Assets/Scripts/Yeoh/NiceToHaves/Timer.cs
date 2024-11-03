using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float secondsLeft=0;
    public bool canTick=true;
    public bool ignoreTimescale;
    
    public void StartTimer(float t) => secondsLeft = t;

    public bool IsTicking() => secondsLeft>0;

    void Update()
    {
        if(!canTick) return;

        float dt = ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;

        if(IsTicking())
        {
            secondsLeft -= dt;

            if(secondsLeft<=0)
            {
                secondsLeft=0;
                TimerFinishedEvent?.Invoke();
            }
        }
    }

    public event Action TimerFinishedEvent;

    public void FinishTimer() => secondsLeft=0;

    public float GetNormalizedTimeLeft(float max)
    {
        if(max<=0 || secondsLeft<=0)
        return 0;

        if(secondsLeft >= max)
        return 1;
        
        return secondsLeft/max;
    }
}
