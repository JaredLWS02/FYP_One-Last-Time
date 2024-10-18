using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]

public class Hurt2D : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        maxPoise=poise;
    }

    // ============================================================================

    public HPManager hp;

    // check block/parry first before hurting

    public void Hurt(GameObject attacker, HurtInfo2D hurtInfo)
    {
        if(iframe) return;

        EventManager.Current.OnHurt(gameObject, attacker, hurtInfo);
        
        hp.Hurt(hurtInfo.damage);

        OnHurt.Invoke();

        if(hp.hp>0) // if still alive
        {
            DoIFraming(iframeTime, .75f, -.75f, -.75f); // flicker red

            HurtPoise(attacker, hurtInfo);
        }
        else
        {
            Knockback(hurtInfo.knockback, hurtInfo.contactPoint);
            
            Die(attacker, hurtInfo);
        }
    }

    // ============================================================================

    [Header("iFrame")]
    public bool iframe;
    public float iframeTime=.5f;

    public void DoIFraming(float t, float r, float g, float b)
    {
        if(iframing_crt!=null) StopCoroutine(iframing_crt);
        iframing_crt = StartCoroutine(IFraming(t, r, g, b));
    }

    Coroutine iframing_crt;
    
    IEnumerator IFraming(float t, float r, float g, float b)
    {
        iframe=true;

        if(colorFlicker)
        ToggleColorFlicker(true, r, g, b);

        yield return new WaitForSeconds(t);

        iframe=false;
        ToggleColorFlicker(false);

        OnIFrameEnd.Invoke();
    }

    public bool colorFlicker=true;
    public float colorFlickerInterval=.05f;

    void ToggleColorFlicker(bool toggle, float r=0, float g=0, float b=0)
    {
        if(colorFlickeringRt!=null) StopCoroutine(colorFlickeringRt);

        if(toggle) colorFlickeringRt = StartCoroutine(ColorFlickering(r, g, b));

        else SpriteManager.Current.RevertColor(gameObject);
    }

    Coroutine colorFlickeringRt;

    IEnumerator ColorFlickering(float r, float g, float b)
    {
        while(true)
        {
            SpriteManager.Current.OffsetColor(gameObject, r, g, b);
            yield return new WaitForSecondsRealtime(colorFlickerInterval);
            SpriteManager.Current.RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(colorFlickerInterval);
        }
    }

    // ============================================================================

    [Header("Poise")]
    public float poise;
    float maxPoise;

    public void HurtPoise(GameObject attacker, HurtInfo2D hurtInfo)
    {
        poise -= hurtInfo.damage;

        lastPoiseDmgTime = Time.time;

        if(poise<=0)
        {
            // restart poise
            poise = maxPoise;

            Knockback(hurtInfo.knockback, hurtInfo.contactPoint);

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

    public void Knockback(float force, Vector3 contactPoint)
    {
        Vector3 kb_dir = rb.transform.position - contactPoint;
        kb_dir.z=0; // no z axis in 2D
        kb_dir.Normalize();

        rb.velocity = Vector3.zero;
        rb.AddForce(kb_dir * force, ForceMode2D.Impulse);
    }

    // ============================================================================

    [Header("Die")]
    public bool iframeOnDeath=true;

    void Die(GameObject killer, HurtInfo2D hurtInfo)
    {
        StopAllCoroutines();

        iframe = iframeOnDeath;

        SpriteManager.Current.RevertColor(gameObject);
        
        OnDeath.Invoke();

        EventManager.Current.OnDeath(gameObject, killer, hurtInfo);
    }

    // ============================================================================

    [Header("Events")]
    public UnityEvent OnHurt;
    public UnityEvent OnPoiseBreak;
    public UnityEvent OnIFrameEnd;
    public UnityEvent OnDeath;
}
