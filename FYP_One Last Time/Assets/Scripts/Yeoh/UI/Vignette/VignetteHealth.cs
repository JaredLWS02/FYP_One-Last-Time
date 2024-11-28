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
    
    void Update()
    {
        if(!hpM) return;
        float inverted_percent = 100 - hpM.GetHPPercent();
        vignetteColor.a = inverted_percent / 100;
        vignette.color = vignetteColor;
    }
}
