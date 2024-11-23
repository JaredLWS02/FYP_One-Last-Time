using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.HurtEvent -= OnHurt;
        EventM.TryIFrameEvent -= OnTryIFrame;
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

        if(iframe && !hurtbox.ignoreIFrame) return;

        hp.Deplete(hurtbox.damage);

        EventM.OnHurted(owner, attacker, hurtbox, contactPoint);
        hurtEvents.OnHurted?.Invoke(contactPoint);

        if(hp.hp>0) // if still alive
        {
            EventM.OnTryIFrame(owner, iframeSeconds);

            TryHurtPoise(attacker, hurtbox, contactPoint);
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

        EventM.OnIFrameToggle(owner, true);
        hurtEvents.OnIFrameToggle?.Invoke(true);

        yield return new WaitForSeconds(t);

        iframe=false;

        ToggleColorFlicker(false);

        EventM.OnIFrameToggle(owner, false);
        hurtEvents.OnIFrameToggle?.Invoke(false);
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

    void TryHurtPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(hurtbox.ignorePoise)
        {
            BreakPoise(attacker, hurtbox, contactPoint);
            return;
        }

        poise -= hurtbox.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            BreakPoise(attacker, hurtbox, contactPoint);
            poise = maxPoise; // reset poise
        }
    }

    void BreakPoise(GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        EventM.OnTryStun(owner, attacker, hurtbox, contactPoint);
        EventM.OnTryKnockback(owner, hurtbox.knockback, contactPoint);
        EventM.OnPoiseBroke(owner, attacker, hurtbox, contactPoint);
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

        hurtEvents.OnDeath?.Invoke(contactPoint);

        if(destroyOnDeath)
        Destroy(owner);
    }

    // ============================================================================

    [System.Serializable]
    public struct HurtEvents
    {
        public UnityEvent<Vector3> OnHurted;
        public UnityEvent<bool> OnIFrameToggle;
        public UnityEvent<Vector3> OnDeath;
    }
    [Space]
    public HurtEvents hurtEvents;
}
