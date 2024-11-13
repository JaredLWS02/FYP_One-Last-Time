using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtScript : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    void Awake()
    {
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
        if(victim!=owner) return;
        if(iframe) return;

        hp.Deplete(hurtbox.damage);

        if(hp.hp>0) // if still alive
        {
            EventM.OnTryIFrame(owner, iframeSeconds);

            HurtPoise(attacker, hurtbox, contactPoint);
        }
        else
        {
            EventM.OnDeath(owner, attacker, hurtbox, contactPoint);
        }
    }

    // ============================================================================

    [Header("iFrame")]
    public float iframeSeconds=.5f;
    public bool iframe {get; private set;}

    void OnTryIFrame(GameObject who, float duration)
    {
        if(who!=owner) return;

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

        EventM.OnIFrameStart(owner);

        yield return new WaitForSeconds(t);

        iframe=false;
        ToggleColorFlicker(false);

        EventM.OnIFrameEnd(owner);
    }

    // ============================================================================
    
    public bool colorFlicker=true;
    public Vector3 rgbOffset = new(.75f, -.75f, -.75f); // red
    public float colorFlickerInterval=.05f;

    void ToggleColorFlicker(bool toggle)
    {
        if(colorFlickering_crt!=null) StopCoroutine(colorFlickering_crt);

        if(toggle) colorFlickering_crt = StartCoroutine(ColorFlickering());

        else RevertColor(owner);
    }

    Coroutine colorFlickering_crt;

    IEnumerator ColorFlickering()
    {
        while(true)
        {
            OffsetColor(owner, rgbOffset.x, rgbOffset.y, rgbOffset.z);
            yield return new WaitForSecondsRealtime(colorFlickerInterval);
            RevertColor(owner);
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

    public bool allowStun=true;

    public void HurtPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        poise -= hurtbox.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            poise = maxPoise; // reset poise

            EventM.OnTryKnockback(owner, hurtbox.knockback, contactPoint);

            if(allowStun && hurtbox.canStun)
            EventM.OnTryStun(owner, attacker, hurtbox, contactPoint);

            EventM.OnPoiseBreak(owner, attacker, hurtbox, contactPoint);
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

    [Header("Knockback")]
    public Rigidbody rb;
    public bool allowKnockback=true;
    public float knockbackMult=1;

    public void OnTryKnockback(GameObject who, float force, Vector3 contactPoint)
    {
        if(who!=owner) return;

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
        if(victim!=owner) return;

        StopAllCoroutines();

        EventM.OnTryKnockback(owner, hurtbox.knockback, contactPoint);

        iframe = iframeOnDeath;

        RevertColor(owner);

        if(destroyOnDeath)
        Destroy(owner);
    }
}
