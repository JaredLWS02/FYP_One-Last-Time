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
        hider=GetComponent<UIBarHider>();
    }

    // Event Manager ============================================================================

    void OnEnable()
    {
        //EventManager.Current.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        //EventManager.Current.UIBarUpdateEvent -= OnUIBarUpdate;

        barTween.Stop();
    }
    
    // ============================================================================

    public GameObject owner;

    public float tweenTime=.2f;
    Tween barTween;

    void OnUIBarUpdate(GameObject who, float value, float valueMax)
    {
        if(!who || who!=owner) return;

        if(valueMax==0) { Debug.LogError($"{gameObject.name}: valueMax can't be 0!!!"); return; }

        float value01 = Mathf.Clamp01(value/valueMax);

        TweenFilledImage(value01, tweenTime);
        TweenSlider(value01, tweenTime);

        hider.currentValue = value;
        hider.maxValue = valueMax;
    }

    // ============================================================================

    [Header("Slider Version")]
    public Slider slider; 

    public void TweenSlider(float to, float time)
    {
        if(!slider) return;

        barTween.Stop();
        barTween = Tween.UISliderValue(slider, to, time, Ease.InOutSine, 1, CycleMode.Restart, 0, 0, true);
    }

    // ============================================================================

    [Header("Radial Bar Version")]
    public Image filledImage; 

    public void TweenFilledImage(float to, float time)
    {
        if(!filledImage) return;

        barTween.Stop();
        barTween = Tween.UIFillAmount(filledImage, to, time, Ease.InOutSine, 1, CycleMode.Restart, 0, 0, true);
    }

}
