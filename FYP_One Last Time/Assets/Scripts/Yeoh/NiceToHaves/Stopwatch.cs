using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stopwatch : MonoBehaviour
{
    bool isTicking;
    float currentTime;
    public int decimalPlaces = 2;
    public bool ignoreTimescale;
    public bool useMillisecondsResult = true;

    void Update()
    {
        if(!isTicking) return;

        float dt = ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
        currentTime += dt;
    }

    public void StartTimer() => isTicking = true;

    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }

    public void StopTimer()
    {
        if(!isTicking) return;
        isTicking = false;

        if(useMillisecondsResult)
            currentTime *= 1000;

        currentTime = Round(currentTime, decimalPlaces);

        OnStop_float?.Invoke(currentTime);
        OnStop_str?.Invoke(currentTime.ToString());
    }

    public void ResetTimer()
    {
        StopTimer();
        currentTime = 0;
    }

    float Round(float num, int decimal_places)
    {
        int factor=1;

        for(int i=0; i<decimal_places; i++)
        {
            factor *= 10;
        }

        return Mathf.Round(num * factor) / factor;
    }

    public UnityEvent<float> OnStop_float;
    public UnityEvent<string> OnStop_str;
}
