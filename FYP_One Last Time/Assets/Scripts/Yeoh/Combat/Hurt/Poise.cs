using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Poise : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public float poise;
    float maxPoise;

    void Awake()
    {
        maxPoise=poise;
    }

    public void TryHurtPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(hurtbox.ignorePoise)
        {
            BreakPoise(attacker, hurtbox, contactPoint);
            return;
        }

        poise -= hurtbox.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            BreakPoise(attacker, hurtbox, contactPoint);
            poise = maxPoise; // reset poise
        }
    }

    void BreakPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        EventM.OnTryStun(owner, attacker, hurtbox, contactPoint);
        EventM.OnTryKnockback(owner, hurtbox.knockback, contactPoint, hurtbox.killsMomentum);
        EventM.OnPoiseBroke(owner, attacker, hurtbox, contactPoint);

        poiseEvents.OnPoiseBreak?.Invoke();
    }

    float lastPoiseDmgTime;
    public float poiseRegenDelay=3;
    
    // ============================================================================

    void Update()
    {
        CheckPoiseRegen();
    }

    void CheckPoiseRegen()
    {
        if(Time.time-lastPoiseDmgTime > poiseRegenDelay)
        {
            // instant fill instead of slowly regen
            poise = maxPoise;
        }
    }

    // ============================================================================

    [System.Serializable]
    public struct PoiseEvents
    {
        public UnityEvent OnPoiseBreak;
    }
    [Space]
    public PoiseEvents poiseEvents;
}
