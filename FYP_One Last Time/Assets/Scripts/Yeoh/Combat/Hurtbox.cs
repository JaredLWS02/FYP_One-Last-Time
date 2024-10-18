using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Hurtbox : MonoBehaviour
{
    public GameObject owner;
    public AttackSO attackSO;
    
    Collider coll;
    Collider2D coll2D;
    public bool enabledOnAwake=true;

    void Awake()
    {
        coll = GetComponent<Collider>();

        ToggleColl(enabledOnAwake);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
    }

    // ============================================================================

    // on successful hit
    void OnHurt(GameObject victim, GameObject attacker, AttackSO attack, Vector3 contantPoint)
    {
        if(owner != attacker) return;
        if(attackSO != attack) return;

        OnHurtt.Invoke();

        if(destroyOnHurt)
        {
            Destroy(gameObject);
            return;
        }

        // decrease first, then check
        if(--attack.pierceCount <= 0)
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

    void OnTriggerStay(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;
        
        contactPoint = other.ClosestPoint(hurtboxOrigin ? hurtboxOrigin.position : transform.position);
        
        AttackSO attack = new(attackSO);

        EventManager.Current.OnTryHurt(owner, otherRb.gameObject, attack, contactPoint);

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
