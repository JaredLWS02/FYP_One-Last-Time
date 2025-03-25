using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GotParriedScript : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ParryCounteredEvent += OnParryCountered;
    }
    void OnDisable()
    {
        EventM.ParryCounteredEvent -= OnParryCountered;
    }

    // ============================================================================

    [Header("When Parried")]
    public bool knockback=true;
    public bool cancelAttack=true;
    public bool parryStun=true;
    public AnimSO optionalParryStunAnim;
    public bool ignoreStunCooldown=true;

    void OnParryCountered(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contact_point)
    {
        if(attacker != owner) return;

        if(knockback)
        {
            EventM.OnTryKnockback(owner, hurtbox.blockKnockback*.5f, contact_point, hurtbox.killsMomentum);
        }

        if(cancelAttack)
        {
            EventM.OnCancelAttack(owner);
            EventM.OnInterruptAttack(owner);
        }

        if(parryStun)
        {
            HurtboxSO stun_info = ScriptableObject.CreateInstance<HurtboxSO>();

            stun_info.canStun = parryStun;
            stun_info.customStunAnim = optionalParryStunAnim;
            stun_info.ignoreStunCooldown = ignoreStunCooldown;

            EventM.OnStun(owner, defender, stun_info, contact_point);
        }

        events.OnParried?.Invoke();
    }

    // ============================================================================

    [System.Serializable]
    public struct UEvents
    {
        public UnityEvent OnParried;
    }
    [Space]
    public UEvents events;
}
