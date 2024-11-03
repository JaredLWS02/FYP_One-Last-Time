using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]

public class HurtScript : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxPoise=poise;
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.HurtEvent += OnHurt;
        EventM.TryIFrameEvent += OnTryIFrame;
        EventM.TryKnockbackEvent += OnTryKnockback;
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;
        EventM.TryIFrameEvent -= OnTryIFrame;
        EventM.TryKnockbackEvent -= OnTryKnockback;
        EventM.DeathEvent -= OnDeath;
    }

    // ============================================================================
    
    ModelManager ModelM;
    SpriteManager SpriteM;

    void Start()
    {
        ModelM = ModelManager.Current;
        SpriteM = SpriteManager.Current;
    }

    // ============================================================================

    public HPManager hp;

    // check block/parry first before hurting

    void OnHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;
        if(iframe) return;

        hp.Deplete(hurtbox.damage);

        if(hp.hp>0) // if still alive
        {
            EventM.OnTryIFrame(gameObject, iframeSeconds);

            HurtPoise(attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnDeath(gameObject, attacker, hurtbox, contactPoint);
        }
    }

    // ============================================================================

    [Header("iFrame")]
    public float iframeSeconds=.5f;
    [HideInInspector]
    public bool iframe;

    void OnTryIFrame(GameObject who, float duration)
    {
        if(who!=gameObject) return;

        if(duration<=0) return;

        if(iframing_crt!=null) StopCoroutine(iframing_crt);
        iframing_crt = StartCoroutine(IFraming(duration));
    }

    Coroutine iframing_crt;
    
    IEnumerator IFraming(float t)
    {
        iframe=true;

        if(colorFlicker)
        ToggleColorFlicker(true);

        EventM.OnIFrameStart(gameObject);

        yield return new WaitForSeconds(t);

        iframe=false;
        ToggleColorFlicker(false);

        EventM.OnIFrameEnd(gameObject);
    }

    // ============================================================================
    
    public bool colorFlicker=true;
    public Vector3 rgbOffset = new(.75f, -.75f, -.75f); // red
    public float colorFlickerInterval=.05f;

    void ToggleColorFlicker(bool toggle)
    {
        if(colorFlickering_crt!=null) StopCoroutine(colorFlickering_crt);

        if(toggle) colorFlickering_crt = StartCoroutine(ColorFlickering());

        else RevertColor(gameObject);
    }

    Coroutine colorFlickering_crt;

    IEnumerator ColorFlickering()
    {
        while(true)
        {
            OffsetColor(gameObject, rgbOffset.x, rgbOffset.y, rgbOffset.z);
            yield return new WaitForSecondsRealtime(colorFlickerInterval);
            RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(colorFlickerInterval);
        }
    }

    void OffsetColor(GameObject target, float r, float g, float b)
    {
        if(ModelM) ModelM.OffsetColor(target, r, g, b);
        if(SpriteM) SpriteM.OffsetColor(target, r, g, b);
    }
    void RevertColor(GameObject target)
    {
        if(ModelM) ModelM.RevertColor(target);
        if(SpriteM) SpriteM.RevertColor(target);
    }
    
    // ============================================================================

    [Header("Poise")]
    public float poise;
    float maxPoise;

    public void HurtPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        poise -= hurtbox.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            poise = maxPoise; // reset poise

            EventM.OnTryKnockback(gameObject, hurtbox.knockback, contactPoint);

            if(allowStun && hurtbox.canStun)
            EventM.OnTryStun(gameObject, attacker, hurtbox, contactPoint);

            EventM.OnPoiseBreak(gameObject, attacker, hurtbox, contactPoint);
        }
    }

    float lastPoiseDmgTime;
    public float poiseRegenDelay=3;
    
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

    public bool allowStun=true;
    public bool allowKnockback=true;
    public float knockbackMult=1;

    public void OnTryKnockback(GameObject who, float force, Vector3 contactPoint)
    {
        if(who!=gameObject) return;

        if(!allowKnockback) return;
        if(knockbackMult==0) return;

        if(rb.isKinematic) return;

        Vector3 kb_dir = (rb.transform.position - contactPoint).normalized;

        if(knockbackMult!=1)
        force *= knockbackMult;

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * force, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("Die")]
    public bool iframeOnDeath=true;
    public bool destroyOnDeath;

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        StopAllCoroutines();

        EventM.OnTryKnockback(gameObject, hurtbox.knockback, contactPoint);

        iframe = iframeOnDeath;

        RevertColor(gameObject);

        if(destroyOnDeath)
        Destroy(gameObject);
    }
}
