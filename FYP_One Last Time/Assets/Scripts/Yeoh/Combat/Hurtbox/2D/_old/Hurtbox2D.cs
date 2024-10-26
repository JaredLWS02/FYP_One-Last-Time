using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]

public class Hurtbox2D : MonoBehaviour
{
    public GameObject owner;
    public HurtboxSO hurtboxSO;
    
    Collider2D coll;
    public bool enabledOnAwake=true;

    void Awake()
    {
        coll = GetComponent<Collider2D>();

        ToggleColl(enabledOnAwake);
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.HurtEvent += OnHurt;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;
    }

    // ============================================================================

    void OnHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contantPoint)
    {
        if(owner != attacker) return;
        if(hurtboxSO != hurtbox) return;

        OnHurtt.Invoke();

        if(destroyOnHurt)
        {
            Destroy(gameObject);
            return;
        }

        // decrease first, then check
        if(--hurtbox.pierceCount <= 0)
        ToggleColl(false);
    }
    
    public void ToggleColl(bool toggle)
    {
        coll.enabled = toggle;
    }

    // ============================================================================

    [Header("Optional")]
    public Transform hurtboxOrigin;
    Vector3 contactPoint;

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.isTrigger) return;
        Rigidbody2D otherRb = other.attachedRigidbody;
        if(!otherRb) return;
        
        contactPoint = other.ClosestPoint(hurtboxOrigin ? hurtboxOrigin.position : transform.position);
        contactPoint.z=0;  // for 2D

        HurtboxSO hurtbox = new(hurtboxSO);

        EventM.OnTryHurt(owner, otherRb.gameObject, hurtbox, contactPoint);

        OnHit.Invoke();

        if(destroyOnHit) Destroy(gameObject);
    }
    
    // ============================================================================

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

    // ============================================================================

    [Space]
    public UnityEvent OnHit;
    public bool destroyOnHit;
    public UnityEvent OnHurtt;
    public bool destroyOnHurt;
}
