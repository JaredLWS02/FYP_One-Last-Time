using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HurtInfo
{
    [HideInInspector]
    public Collider coll;
    public GameObject owner;

    [Header("On Hit")]
    public float damage=10;
    public float damageBlock=5;
    public float knockback=5;
    public float stunSeconds=1;
    public float stunSpeedMult=.3f;
    [HideInInspector]
    public Vector3 contactPoint;
    public bool hasSweepingEdge=true;
    public bool unparryable;

    [Header("Optional")]
    public Transform hurtboxOrigin;

    // ctor
    public HurtInfo(HurtInfo hurtInfo)
    {
        coll = hurtInfo.coll;
        owner = hurtInfo.owner;

        damage = hurtInfo.damage;
        damageBlock = hurtInfo.damageBlock;
        knockback = hurtInfo.knockback;
        stunSeconds = hurtInfo.stunSeconds;
        stunSpeedMult = hurtInfo.stunSpeedMult;
        contactPoint = hurtInfo.contactPoint;
        hasSweepingEdge = hurtInfo.hasSweepingEdge;
        unparryable = hurtInfo.unparryable;

        hurtboxOrigin = hurtInfo.hurtboxOrigin;
    }
}

// ============================================================================

[RequireComponent(typeof(Collider))]

public class Hurtbox : MonoBehaviour
{
    [Header("Edit This")]
    public HurtInfo myHurtInfo;

    [Space][Space][Space]

    public bool enabledOnAwake=true;

    void Awake()
    {
        myHurtInfo.coll = GetComponent<Collider>();

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

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        // ignore if not this hurtbox
        if(hurtInfo.coll != myHurtInfo.coll) return;

        // if can swipe through multiple
        ToggleColl(myHurtInfo.hasSweepingEdge); 

        OnHit.Invoke();
        if(destroyOnHit) Destroy(gameObject);
    }
    
    // ============================================================================

    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;
        
        Hit(other, otherRb);
    }

    // ============================================================================

    void Hit(Collider other, Rigidbody otherRb)
    {
        Transform origin = myHurtInfo.hurtboxOrigin;

        HurtInfo hurtInfo = new(myHurtInfo);

        hurtInfo.contactPoint = other.ClosestPoint(origin ? origin.position : transform.position);
        
        EventManager.Current.OnHit(myHurtInfo.owner, otherRb.gameObject, hurtInfo);
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

    public void ToggleColl(bool toggle)
    {
        myHurtInfo.coll.enabled=toggle;
    }

    // ============================================================================

    [Space]
    public UnityEvent OnHit;
    public bool destroyOnHit;
}
