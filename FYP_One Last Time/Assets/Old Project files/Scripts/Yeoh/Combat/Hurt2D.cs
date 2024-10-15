using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Hurt2D : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();

        maxPoise=poise;
    }

    void Update()
    {
        CheckPoiseRegen();
    }

    // ============================================================================

    public HPManager hp;

    public void Hurt(GameObject attacker, HurtInfo hurtInfo)
    {
        if(iframe) return;

        hurtInfo.victimName = gameObject.name;

        EventManager.Current.OnHurt(gameObject, attacker, hurtInfo);
        
        hp.Hurt(hurtInfo.dmg);

        if(hp.hp>0) // if still alive
        {
            DoIFraming(iframeTime, .75f, -.75f, -.75f); // flicker red

            HurtPoise(attacker, hurtInfo);
        }
        else
        {
            Knockback(hurtInfo.kbForce, hurtInfo.contactPoint);
            
            Die(attacker, hurtInfo);
        }
    }

    // ============================================================================

    [Header("iFrame")]
    public bool iframe;
    public float iframeTime=.5f;
    public bool colorFlicker=true;

    public void DoIFraming(float t, float r, float g, float b)
    {
        if(iFramingRt!=null) StopCoroutine(iFramingRt);
        iFramingRt = StartCoroutine(IFraming(t, r, g, b));
    }

    Coroutine iFramingRt;
    
    IEnumerator IFraming(float t, float r, float g, float b)
    {
        iframe=true;
        if(colorFlicker) ToggleColorFlicker(true, r, g, b);

        yield return new WaitForSeconds(t);

        iframe=false;
        if(colorFlicker) ToggleColorFlicker(false);
    }

    void ToggleColorFlicker(bool toggle, float r=0, float g=0, float b=0)
    {
        if(colorFlickeringRt!=null) StopCoroutine(colorFlickeringRt);

        if(toggle)
        {
            colorFlickeringRt = StartCoroutine(ColorFlickering(r, g, b));
        }
        else SpriteManager.Current.RevertColor(gameObject);
    }

    Coroutine colorFlickeringRt;

    IEnumerator ColorFlickering(float r, float g, float b)
    {
        while(true)
        {
            SpriteManager.Current.OffsetColor(gameObject, r, g, b);
            yield return new WaitForSecondsRealtime(.05f);
            SpriteManager.Current.RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    // ============================================================================

    [Header("Poise")]
    public float poise;
    float maxPoise;

    public void HurtPoise(GameObject attacker, HurtInfo hurtInfo)
    {
        poise-=hurtInfo.dmg;

        lastPoiseDmgTime=Time.time;

        if(poise<=0)
        {
            poise=maxPoise;

            //EventManager.Current.OnStun(gameObject, attacker, hurtInfo);

            Knockback(hurtInfo.kbForce, hurtInfo.contactPoint);
        }
    }

    float lastPoiseDmgTime;
    public float poiseRegenDelay=3;
    
    void CheckPoiseRegen()
    {
        if(Time.time-lastPoiseDmgTime > poiseRegenDelay)
        {
            if(poise<maxPoise)
            {
                poise=maxPoise; // instant fill instead of slowly regen
            }
        }
    }

    // ============================================================================

    public void Knockback(float force, Vector3 contactPoint)
    {
        Vector3 kbVector = rb.transform.position - contactPoint;
        kbVector.z=0; // no z axis in 2D

        rb.velocity = Vector3.zero;
        rb.AddForce(kbVector.normalized * force, ForceMode2D.Impulse);
    }

    // ============================================================================

    void Die(GameObject killer, HurtInfo hurtInfo)
    {
        SpriteManager.Current.RevertColor(gameObject);
        
        hurtInfo.victimName = gameObject.name;

        EventManager.Current.OnDeath(gameObject, killer, hurtInfo);
    }
}
