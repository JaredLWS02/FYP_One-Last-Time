using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HurtInfo2D
{
    [HideInInspector]
    public Collider2D coll;
    public GameObject owner;

    [Header("On Hit")]
    public float damage;
    public float knockback;
    public Vector3 contactPoint;
    [HideInInspector]
    public bool hasSweepingEdge;

    [Header("Optional")]
    public Transform hurtboxOrigin;
}

// ============================================================================

[RequireComponent(typeof(Collider2D))]

public class Hurtbox2D : MonoBehaviour
{
    [Header("Edit This")]
    public HurtInfo2D myHurtInfo;

    [Space][Space][Space]

    public bool enabledOnAwake=true;

    void Awake()
    {
        myHurtInfo.coll = GetComponent<Collider2D>();

        ToggleColl(enabledOnAwake);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.Hurt2DEvent += OnHurt;
    }
    void OnDisable()
    {
        EventManager.Current.Hurt2DEvent -= OnHurt;
    }

    // ============================================================================

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo2D hurtInfo)
    {
        // ignore if not this hurtbox
        if(hurtInfo.coll != myHurtInfo.coll) return;

        // if can swipe through multiple
        ToggleColl(myHurtInfo.hasSweepingEdge); 

        OnHit.Invoke();
        if(destroyOnHit) Destroy(gameObject);
    }
    
    // ============================================================================

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.isTrigger) return;
        Rigidbody2D otherRb = other.attachedRigidbody;
        if(!otherRb) return;
        
        Hit(other, otherRb);
    }

    // ============================================================================

    void Hit(Collider2D other, Rigidbody2D otherRb)
    {
        Transform origin = myHurtInfo.hurtboxOrigin;

        myHurtInfo.contactPoint = other.ClosestPoint(origin ? origin.position : transform.position);

        myHurtInfo.contactPoint.z=0; // for 2D
        
        EventManager.Current.OnHit(myHurtInfo.owner, otherRb.gameObject, myHurtInfo);
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
