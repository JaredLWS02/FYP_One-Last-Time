using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Hurtbox : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;
    
    // ============================================================================

    Collider coll;
    
    public bool enabledOnAwake=true;

    void Awake()
    {
        coll = GetComponent<Collider>();

        ToggleColl(enabledOnAwake);
    }
    
    // ============================================================================

    public HurtboxSO hurtboxSO;

    [Header("Optional")]
    public Transform hurtboxOrigin;

    Vector3 contactPoint;

    void OnTriggerStay(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;

        Vector3 origin = hurtboxOrigin ? hurtboxOrigin.position : transform.position;
        
        contactPoint = other.ClosestPoint(origin);
        
        HurtboxSO new_hurtbox = HurtboxSO.CreateInstance(hurtboxSO);

        EventM.OnTryHurt(otherRb.gameObject, owner, new_hurtbox, contactPoint);

        uEvents.Hit?.Invoke();

        if(destroyOnHit) Destroy(gameObject);
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.HurtEvent += OnHurt;

        EventM.AttackCancelledEvent += OnAttackCancelled;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;

        EventM.AttackCancelledEvent -= OnAttackCancelled;
    }

    // ============================================================================

    // on successful hit
    void OnHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contantPoint)
    {
        if(owner != attacker) return;
        // this.hurtboxSO is NOT an instance
        // hurtbox param is an instance (clone)
        // they are different
        if(hurtboxSO.Name != hurtbox.Name) return;

        // decrease first, then check
        if(--hurtbox.pierceCount <= 0)
        ToggleColl(false); 

        uEvents.Hurt?.Invoke();

        if(destroyOnHurt) Destroy(gameObject);
    }   

    public void ToggleColl(bool toggle)
    {
        coll.enabled = toggle;
    }

    // ============================================================================

    void OnAttackCancelled(GameObject who)
    {
        if(who!=owner) return;

        ToggleColl(false);
    }

    // ============================================================================

    [System.Serializable]
    public struct UEvents
    {
        public UnityEvent Hit;
        public UnityEvent Hurt;
    }
    
    public UEvents uEvents;

    public bool destroyOnHit;
    public bool destroyOnHurt;









    // Old ============================================================================

    public void Blink(float time=.1f)
    {
        if(time>0)
        {
            if(blinking_crt!=null) StopCoroutine(blinking_crt);
            blinking_crt = StartCoroutine(Blinking(time)); 
        }
    }

    Coroutine blinking_crt;

    IEnumerator Blinking(float t)
    {
        ToggleColl(true);
        yield return new WaitForSeconds(t);
        ToggleColl(false);
    }
}
