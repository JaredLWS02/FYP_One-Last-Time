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
    
    // ============================================================================

    void Update()
    {
        if(slider) hider.value = slider.value;
        else if(filledImage) hider.value = filledImage.fillAmount;
        hider.maxValue = 1;
    }

    // Event Manager ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        EventM.UIBarUpdateEvent -= OnUIBarUpdate;

        sliderTween.Stop();
    }
    
    // ============================================================================

    public GameObject owner;

    public float tweenTime=.2f;
    public bool ignoreTime=true;

    void OnUIBarUpdate(GameObject who, float value, float maxValue)
    {
        if(!who || who!=owner) return;

        if(maxValue==0) { Debug.LogError($"{gameObject.name}: maxValue can't be 0!!!"); return; }

        float value01 = Mathf.Clamp01(value/maxValue);

        TweenSlider(value01, tweenTime);
        TweenFill(value01, tweenTime);

        USE SMOOTH DAMP!!!!!!!!!!
    }

    // ============================================================================

    [Header("Slider Version")]
    public Slider slider; 

    Tween sliderTween;

    public void TweenSlider(float to, float time)
    {
        if(!slider) return;

        sliderTween.Stop();
        if(time>0) sliderTween = Tween.UISliderValue(slider, to, time, Ease.InOutSine, useUnscaledTime: ignoreTime);
        else slider.value = to;
    }

    // ============================================================================

    [Header("Radial Bar Version")]
    public Image filledImage; 

    Tween fillTween;

    public void TweenFill(float to, float time)
    {
        if(!filledImage) return;

        fillTween.Stop();
        if(time>0) fillTween = Tween.UIFillAmount(filledImage, to, time, Ease.InOutSine, useUnscaledTime: ignoreTime);
        else filledImage.fillAmount = to;
    }
}
