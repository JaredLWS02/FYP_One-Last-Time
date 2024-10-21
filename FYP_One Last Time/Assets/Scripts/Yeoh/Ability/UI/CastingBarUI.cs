using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CastingBarUI : MonoBehaviour
{
    public GameObject owner;
    public UIBarTween tween;

    public bool enableBarOnAwake = true;

    void Awake()
    {
        if(enableBarOnAwake)
        tween.gameObject.SetActive(true);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.CastingEvent += OnCasting;
        EventManager.Current.CastCancelEvent += OnCastCancel;

        ResetBar(0);
    }
    void OnDisable()
    {
        EventManager.Current.CastingEvent -= OnCasting;
        EventManager.Current.CastCancelEvent -= OnCastCancel;

        ResetBar(0);
    }

    // ============================================================================

    void OnCasting(GameObject caster, AbilitySlot slot)
    {
        if(caster!=owner) return;

        ResetBar(0);
        FillBar(slot.ability.castingTime);
    }

    void OnCastCancel(GameObject caster)
    {
        if(caster!=owner) return;

        ResetBar(.1f);
    }
    
    // ============================================================================

    void ResetBar(float time)
    {
        tween.tweenTime = time;
        EventManager.Current.OnUIBarUpdate(gameObject, 0, 1);
    }
    void FillBar(float time)
    {
        tween.tweenTime = time;
        EventManager.Current.OnUIBarUpdate(gameObject, 1, 1);
    }
}
