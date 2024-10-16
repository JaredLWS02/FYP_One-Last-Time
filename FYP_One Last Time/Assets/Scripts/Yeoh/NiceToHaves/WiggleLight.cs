using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]

public class WiggleLight : MonoBehaviour
{
    Light myLight;
    
    float defaultValue;
    float seed;

    void Awake()
    {
        myLight = GetComponent<Light>();

        defaultValue = myLight.intensity;
        seed = Random.value*99999;

        min = offset;
    }

    // ============================================================================

    [Header("Wiggle")]
    public bool wiggle=true;

    public float frequency=3;
    public float magnitude=4;
    public float offset=4;

    public bool ignoreTime=true;
    
    void Update()
    {
        if(!wiggle) return;

        float time = ignoreTime ? Time.unscaledTime : Time.time;

        float noise = (Mathf.PerlinNoise(seed, time * frequency) * 2-1) * magnitude + offset;
        myLight.intensity = noise;

        DebugValue(noise);
    }

    // ============================================================================

    [Header("Debug")]
    public float min;
    public float current;
    public float max;

    void DebugValue(float value)
    {
        current = value;

        if(current>max) max = current;
        if(current<min) min = current;
    }

    // ============================================================================

    public void Shake(float duration)
    {
        if(shaking_crt!=null) StopCoroutine(shaking_crt);
        shaking_crt = StartCoroutine(Shaking(duration));
    }

    Coroutine shaking_crt;
    IEnumerator Shaking(float t)
    {
        myLight.intensity = defaultValue;
        wiggle=true;
        yield return new WaitForSeconds(t);
        wiggle=false;
        myLight.intensity = defaultValue;
    }
}
