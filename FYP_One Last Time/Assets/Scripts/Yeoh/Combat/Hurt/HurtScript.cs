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
        EventM.KnockbackEvent += OnKnockback;
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;
        EventM.KnockbackEvent -= OnKnockback;
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

        OnHurtt.Invoke();

        if(hp.hp>0) // if still alive
        {
            DoIFraming(iframeSeconds);

            HurtPoise(attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnKnockback(gameObject, attacker, hurtbox, contactPoint);
            
            EventM.OnDeath(gameObject, attacker, hurtbox, contactPoint);
        }
    }

    // ============================================================================

    [Header("iFrame")]
    public float iframeSeconds=.5f;
    [HideInInspector]
    public bool iframe;

    public void DoIFraming(float t)
    {
        if(iframing_crt!=null) StopCoroutine(iframing_crt);
        iframing_crt = StartCoroutine(IFraming(t));
    }

    Coroutine iframing_crt;
    
    IEnumerator IFraming(float t)
    {
        iframe=true;

        if(colorFlicker)
        ToggleColorFlicker(true);

        yield return new WaitForSeconds(t);

        iframe=false;
        ToggleColorFlicker(false);

        OnIFrameEnd.Invoke();
    }

    // ============================================================================
    
    public bool colorFlicker=true;
    public Vector3 rgbOffset = new(.75f, -.75f, -.75f); // red
    public float colorFlickerInterval=.05f;

    void ToggleColorFlicker(bool toggle)
    {
        if(colorFlickeringRt!=null) StopCoroutine(colorFlickeringRt);

        if(toggle) colorFlickeringRt = StartCoroutine(ColorFlickering());

        else RevertColor(gameObject);
    }

    Coroutine colorFlickeringRt;

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

            EventM.OnKnockback(gameObject, attacker, hurtbox, contactPoint);

            EventM.OnTryStun(gameObject, attacker, hurtbox, contactPoint);

            OnPoiseBreak.Invoke();
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

    public void OnKnockback(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        Vector3 kb_dir = (rb.transform.position - contactPoint).normalized;

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * hurtbox.knockback, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("Die")]
    public bool iframeOnDeath=true;

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        StopAllCoroutines();

        iframe = iframeOnDeath;

        RevertColor(gameObject);
        
        OnDeathh.Invoke();
    }

    // ============================================================================

    [Header("Events")]
    public UnityEvent OnHurtt;
    public UnityEvent OnPoiseBreak;
    public UnityEvent OnIFrameEnd;
    public UnityEvent OnDeathh;
}
