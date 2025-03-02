using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class TweenAnim2 : MonoBehaviour
{
    public Transform target;

    Vector3 defaultPos;
    Vector3 defaultRot;
    Vector3 defaultScale;

    void Start()
    {
        RecordDefaults();
    }

    public void RecordDefaults()
    {
        defaultPos = target.position;
        defaultRot = target.eulerAngles;
        defaultScale = target.localScale;
    }

    // ============================================================================

    public List<TweenSettings<Vector3>> vector3Tweens = new();
    //public List<TweenSettings<float>> floatTweens = new();

    // ============================================================================

    public void TweenPosition(int settings_index)
    {
        var cfg = vector3Tweens[settings_index];
        Tween.Position(target, cfg);
    }

    public void TweenLocalPosition(int settings_index)
    {
        var cfg = vector3Tweens[settings_index];
        Tween.LocalPosition(target, cfg);
    }

    public void ReturnToDefaultPosition()
    {
        target.position = defaultPos;
    }

    // ============================================================================
    
    public void TweenRotation(int settings_index)
    {
        var cfg = vector3Tweens[settings_index];
        Tween.EulerAngles(target, cfg);
    }

    public void TweenLocalRotation(int settings_index)
    {
        var cfg = vector3Tweens[settings_index];
        Tween.LocalEulerAngles(target, cfg);
    }

    public void ReturnToDefaultRotation()
    {
        target.eulerAngles = defaultRot;
    }

    // ============================================================================
    
    public void TweenScale(int settings_index)
    {
        var cfg = vector3Tweens[settings_index];
        Tween.Scale(target, cfg);
    }

    public void ReturnToDefaultScale()
    {
        target.localScale = defaultScale;
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;

    // ============================================================================

    void OnEnable()
    {
        events.OnEnable?.Invoke();
    }
    void OnDisable()
    {
        events.OnDisable?.Invoke();
    }
}
