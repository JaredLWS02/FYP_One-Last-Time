using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarUpdater : MonoBehaviour
{
    public GameObject owner;

    UIBarHider hider;

    void Awake()
    {
        hider = GetComponent<UIBarHider>();

        roundedValue = targetValue = currentValue;
    }
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.UIBarUpdateEvent += OnUIBarUpdate;
    }
    void OnDisable()
    {
        EventM.UIBarUpdateEvent -= OnUIBarUpdate;
    }
    
    // ============================================================================
    
    float targetValue;

    void OnUIBarUpdate(GameObject who, float value, float maxValue)
    {
        if(who!=owner) return;

        if(maxValue==0) { Debug.LogError($"UI Bar Updater: maxValue can't be 0!!!"); return; }

        float value01 = Mathf.Clamp01(value/maxValue);
        
        targetValue = value01;
    }

    // ============================================================================

    void Update()
    {
        UpdateCurrentValue();
        UpdateSlider();
        UpdateFill();

        UpdateHider();
    }

    void UpdateHider()
    {
        if(!hider) return;

        if(slider) hider.value = slider.value;
        else if(filledImage) hider.value = filledImage.fillAmount;
        hider.maxValue = 1;
    }

    // ============================================================================
    
    [Range(0,1)]
    public float currentValue=1;
    float roundedValue=1;

    [Header("Lerp")]
    public bool lerp;
    public float lerpSpeed=2;

    [Header("Damp")]
    public bool damp=true;
    public float dampSeconds=.3f;
    float dampVelocity;

    [Header("Both")]
    public bool ignoreTime=true;

    void UpdateCurrentValue()
    {
        float dt = ignoreTime ? Time.unscaledDeltaTime : Time.deltaTime;

        if(lerp)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, lerpSpeed * dt);
        }
        else if(damp)
        {
            currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref dampVelocity, dampSeconds, Mathf.Infinity, dt);
        }
        else
        {
            currentValue = targetValue;
        }

        roundedValue = Round(currentValue, 2);
    }

    // ============================================================================
    
    public float Round(float num, int decimalPlaces)
    {
        int factor=1;

        for(int i=0; i<decimalPlaces; i++)
        {
            factor *= 10;
        }
        return Mathf.Round(num * factor) / factor;
    }

    // ============================================================================

    [Header("Slider Version")]
    public Slider slider; 

    public void UpdateSlider()
    {
        if(!slider) return;

        slider.value = roundedValue;
    }

    // ============================================================================

    [Header("Radial Bar Version")]
    public Image filledImage; 

    public void UpdateFill()
    {
        if(!filledImage) return;

        filledImage.fillAmount = roundedValue;
    }
}
