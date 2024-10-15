using System.Collections;
using System.Collections.Generic;
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
        EventManager.Current.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        EventManager.Current.UIBarUpdateEvent -= OnUIBarUpdate;

        LeanTween.cancel(gameObject);
    }
    
    public GameObject owner;
    public float tweenTime=.2f;

    void OnUIBarUpdate(GameObject who, float value, float valueMax)
    {
        if(!who || who!=owner) return;

        float normalized_value = value/valueMax;

        TweenFilledImage(normalized_value, tweenTime);
        TweenSlider(normalized_value, tweenTime);

        hider.currentValue = value;
        hider.maxValue = valueMax;
    }

    // Slider Version ============================================================================

    [Header("Slider Version")]
    public Slider slider; 

    int tweenSliderId=0;

    public void TweenSlider(float to, float time)
    {
        if(!slider) return;

        LeanTween.cancel(tweenSliderId);

        if(time>0)
        {
            tweenSliderId = LeanTween.value(slider.value, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{if(slider) slider.value=value;} )
                .id;
        }
        else slider.value=to;
    }

    // Radial Bar Version ============================================================================

    [Header("Radial Bar Version")]
    public Image filledImage; 

    int tweenFilledImageId=0;

    public void TweenFilledImage(float to, float time)
    {
        if(!filledImage) return;

        LeanTween.cancel(tweenFilledImageId);

        if(time>0)
        {
            tweenFilledImageId = LeanTween.value(filledImage.fillAmount, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{if(filledImage) filledImage.fillAmount=value;} )
                .id;
        }
        else filledImage.fillAmount=to;
    }

    
}
