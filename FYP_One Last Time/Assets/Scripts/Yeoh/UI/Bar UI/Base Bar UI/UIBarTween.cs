using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIBarHider))]

public class UIBarTween : MonoBehaviour
{
    UIBarHider hider;

    void Awake()
    {
        hider = GetComponent<UIBarHider>();
    }

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        EventManager.Current.UIBarUpdateEvent -= OnUIBarUpdate;

        barTween.Stop();
    }
    
    // ============================================================================

    public GameObject owner;

    public float tweenTime=.2f;
    public bool ignoreTime=true;

    Tween barTween;

    void OnUIBarUpdate(GameObject who, float value, float maxValue)
    {
        if(!who || who!=owner) return;

        if(maxValue==0) { Debug.LogError($"{gameObject.name}: maxValue can't be 0!!!"); return; }

        float value01 = Mathf.Clamp01(value/maxValue);

        TweenFill(value01, tweenTime);
        TweenSlider(value01, tweenTime);
    }

    // ============================================================================

    [Header("Slider Version")]
    public Slider slider; 

    public void TweenSlider(float to, float time)
    {
        if(!slider) return;

        barTween.Stop();
        if(time>0) barTween = Tween.UISliderValue(slider, to, time, Ease.InOutSine, useUnscaledTime: ignoreTime);
        else slider.value = to;
    }

    // ============================================================================

    [Header("Radial Bar Version")]
    public Image filledImage; 

    public void TweenFill(float to, float time)
    {
        if(!filledImage) return;

        barTween.Stop();
        if(time>0) barTween = Tween.UIFillAmount(filledImage, to, time, Ease.InOutSine, useUnscaledTime: ignoreTime);
        else filledImage.fillAmount = to;
    }

    // ============================================================================

    void Update()
    {
        if(slider) hider.value = slider.value;
        if(filledImage) hider.value = filledImage.fillAmount;
        hider.maxValue = 1;
    }
}
