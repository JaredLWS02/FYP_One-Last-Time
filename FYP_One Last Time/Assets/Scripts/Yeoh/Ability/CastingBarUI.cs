using System.Collections;
using System.Collections.Generic;
using PrimeTween;
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

        progressTween.Stop();
        progress=0;
    }

    // Events ============================================================================

    void OnCasting(GameObject caster, AbilitySlot ability)
    {
        if(caster!=owner) return;

        progress=0;
        barUI.SetActive(true);
        TweenProgress(1, ability.ability.castingTime);
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

    Tween progressTween;

    public void TweenProgress(float to, float time)
    {
        progressTween.Stop();
        progressTween = Tween.Custom(progress, to, time, onValueChange: newVal => progress=newVal, Ease.InOutSine, useUnscaledTime: true);
    }

    // ============================================================================

    float progress;

    void Update()
    {
        EventManager.Current.OnUIBarUpdate(gameObject, progress, 1);
    }
    
}
