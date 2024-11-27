using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtScript : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.HurtEvent += OnHurt;
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;
        EventM.DeathEvent -= OnDeath;
    }

    // ============================================================================

    public HPManager hpM;
    public IFrame iframe;
    public Poise poise;

    // check block/parry first before hurting

    void OnHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(iframe.isActive && !hurtbox.ignoreIFrame) return;

        hpM.Deplete(hurtbox.damage);

        EventM.OnHurted(owner, attacker, hurtbox, contactPoint);
        hurtEvents.OnHurted?.Invoke(contactPoint);

        if(hpM.hp>0) // if still alive
        {
            EventM.OnTryIFrame(owner, iframe.seconds);

            poise.TryHurtPoise(attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnDeath(owner, attacker, hurtbox, contactPoint);
        }
    }    

    // ============================================================================

    [Header("Die")]
    public bool destroyOnDeath;

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        StopAllCoroutines();

        EventM.OnTryKnockback(owner, hurtbox.knockback, contactPoint);

        hurtEvents.OnDeath?.Invoke(contactPoint);

        if(destroyOnDeath)
        Destroy(owner);
    }

    // ============================================================================

    [System.Serializable]
    public struct HurtEvents
    {
        public UnityEvent<Vector3> OnHurted;
        public UnityEvent<Vector3> OnDeath;
    }
    [Space]
    public HurtEvents hurtEvents;
}
