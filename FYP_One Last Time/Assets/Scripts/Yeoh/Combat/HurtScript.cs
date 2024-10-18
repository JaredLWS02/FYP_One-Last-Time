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

    void OnEnable()
    {
        EventManager.Current.HurtEvent += OnHurt;
        EventManager.Current.KnockbackEvent += OnKnockback;
        EventManager.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventManager.Current.HurtEvent -= OnHurt;
        EventManager.Current.KnockbackEvent -= OnKnockback;
        EventManager.Current.DeathEvent -= OnDeath;
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

    public void OnHurt(GameObject victim, GameObject attacker, AttackSO attack, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;
        if(iframe) return;

        hp.Hurt(attack.damage);

        EventManager.Current.OnHurt(gameObject, attacker, attack, contactPoint);

        OnHurtt.Invoke();

        if(hp.hp>0) // if still alive
        {
            DoIFraming(iframeSeconds);

            HurtPoise(attacker, attack, contactPoint);
        }
        else
        {
            EventManager.Current.OnKnockback(gameObject, attacker, attack, contactPoint);
            
            EventManager.Current.OnDeath(gameObject, attacker, attack, contactPoint);
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

    public void HurtPoise(GameObject attacker, AttackSO attack, Vector3 contactPoint)
    {
        poise -= attack.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            poise = maxPoise; // reset poise

            EventManager.Current.OnKnockback(gameObject, attacker, attack, contactPoint);

            OnPoiseBreak.Invoke();

            //EventManager.Current.OnStun(gameObject, attacker, hurtInfo);
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

    public void OnKnockback(GameObject victim, GameObject attacker, AttackSO attack, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        Vector3 kb_dir = (rb.transform.position - contactPoint).normalized;

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * attack.knockback, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("Die")]
    public bool iframeOnDeath=true;

    void OnDeath(GameObject victim, GameObject killer, AttackSO attack, Vector3 contactPoint)
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
