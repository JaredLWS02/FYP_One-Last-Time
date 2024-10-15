using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingBarUI : MonoBehaviour
{
    public GameObject owner;
    public GameObject barUI;

    void Awake()
    {
        barUI.SetActive(false);
    }

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.CastingEvent += OnCasting;
        EventManager.Current.CastCancelEvent += OnCastCancel;
        EventManager.Current.CastWindUpEvent += OnCastingWindUp;
    }
    void OnDisable()
    {
        EventManager.Current.CastingEvent -= OnCasting;
        EventManager.Current.CastCancelEvent -= OnCastCancel;
        EventManager.Current.CastWindUpEvent -= OnCastingWindUp;
    }

    // Events ============================================================================

    void OnCasting(GameObject caster, AbilitySlot ability)
    {
        if(caster!=owner) return;

        progress=0;
        barUI.SetActive(true);
        TweenFloat(1, ability.ability.castingTime);
    }

    void OnCastCancel(GameObject caster)
    {
        if(caster!=owner) return;

        progress=0;
        barUI.SetActive(false);
    }

    void OnCastingWindUp(GameObject caster, AbilitySlot ability)
    {
        if(caster!=owner) return;

        progress=0;
        barUI.SetActive(false);
    }

    // ============================================================================

    void Update()
    {
        EventManager.Current.OnUIBarUpdate(gameObject, progress, 1);
    }

    // Tween Float ============================================================================

    float progress;

    int tweenFloatId=0;

    public void TweenFloat(float to, float time)
    {
        LeanTween.cancel(tweenFloatId);

        if(time>0)
        {
            tweenFloatId = LeanTween.value(progress, to, time)
                .setEaseInOutSine()
                .setIgnoreTimeScale(true)
                .setOnUpdate( (float value)=>{progress=value;} )
                .id;
        }
        else progress=to;
    }
    
}
