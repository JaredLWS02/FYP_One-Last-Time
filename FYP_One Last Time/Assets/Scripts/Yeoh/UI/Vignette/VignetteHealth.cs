using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteHealth : MonoBehaviour
{
    Image vignette;

    void Awake()
    {
        vignette = GetComponent<Image>();
    }

    // ============================================================================

    public HPManager hpM;
    public Color vignetteColor = Color.red;
    public float minAlpha=0;
    public float maxAlpha_=.75f;
    
    void Update()
    {
        if(!hpM) return;

        float hp01 = GetValue01(hpM.hp, 0, hpM.hpMax);
        vignetteColor.a = Mathf.Lerp(minAlpha, maxAlpha_, 1-hp01);

        vignette.color = vignetteColor;
    }

    // ============================================================================

    float GetValue01(float current, float min, float max)
    {
        if(current <= min) return 0;
        if(current >= max) return 1;
        
        float range = max - min;
        if(range<=0) return 0;

        float offset = current - min;
        float value01 = offset / range;

        return Mathf.Clamp01(value01);
    }
}
