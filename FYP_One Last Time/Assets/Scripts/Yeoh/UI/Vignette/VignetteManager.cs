using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class VignetteManager : MonoBehaviour
{
    Image vignette;

    void Awake()
    {
        vignette = GetComponent<Image>();
    }

    // ============================================================================

    Tween alphaTween;

    void TweenAlpha(float to, float time)
    {
        alphaTween.Stop();
        
        if(time>0) alphaTween = Tween.Alpha(vignette, to, time, Ease.InOutSine, useUnscaledTime: true);
        else
        {
            Color color = vignette.color;
            color.a = to;
            vignette.color = color;
        }
    }

    // ============================================================================

    bool canFlash=true;

    public void FlashVignette(Color color, float fade_in=.01f, float wait=0, float fade_out=.5f)
    {
        if(canFlash)
        {
            if(flashing_crt!=null) StopCoroutine(flashing_crt);
            flashing_crt=StartCoroutine(Flashing(color, fade_in, wait, fade_out));
        }
    }

    Coroutine flashing_crt;

    IEnumerator Flashing(Color color, float fade_in, float wait, float fade_out)
    {
        vignette.color = color;

        TweenAlpha(1, fade_in);
        yield return new WaitForSeconds(fade_in);
        yield return new WaitForSeconds(wait);
        TweenAlpha(0, fade_out);
    }

    // ============================================================================

    public void TweenVignette(Color color, float to_alpha, float time)
    {
        if(flashing_crt!=null) StopCoroutine(flashing_crt);

        vignette.color = color;
        TweenAlpha(to_alpha, time);

        canFlash = to_alpha <= 0;
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.HurtedEvent += OnHurted;
        EventM.HealEvent += OnHeal;
        //EventM.BlockEvent += OnBlock;
        EventM.ParryEvent += OnParry;
        EventM.DeathEvent += OnDeath;
        //EventM.RespawnEvent += OnRespawn;
        //EventM.AbilitySlowMoEvent += OnAbilitySlowMo;
        //EventM.AbilityCastEvent += OnAbilityCast;
        //EventM.AbilityEndEvent += OnAbilityEnd;
    }
    void OnDisable()
    {
        EventM.HurtedEvent -= OnHurted;
        EventM.HealEvent -= OnHeal;
        //EventM.BlockEvent -= OnBlock;
        EventM.ParryEvent -= OnParry;
        EventM.DeathEvent -= OnDeath;
        //EventM.RespawnEvent -= OnRespawn;
        //EventM.AbilitySlowMoEvent -= OnAbilitySlowMo;
        //EventM.AbilityCastEvent -= OnAbilityCast;
        //EventM.AbilityEndEvent -= OnAbilityEnd;
    }

    void OnHurted(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim.CompareTag("Player"))
        {
            FlashVignette(Color.red);
        }
    }

    void OnHeal(GameObject who, GameObject healer, float amount)
    {
        if(who.CompareTag("Player"))
        {
            FlashVignette(Color.green);
        }
    }

    // void OnBlock(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    // {
    //     if(defender.CompareTag("Player"))
    //     {
    //         FlashVignette(Color.cyan);
    //     }
    // }

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender.CompareTag("Player"))
        {
            FlashVignette(Color.green);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim.CompareTag("Player"))
        {
            TweenVignette(Color.red, 1, .1f);
        }
    }

    // void OnRespawn(GameObject zombo)
    // {
    //     if(zombo.tag!="Player") return;

    //     TweenVignette(Color.red, 0, .1f);
    // }

    // void OnAbilitySlowMo(bool toggle)
    // {
    //     if(toggle)
    //     {
    //         TweenVignette(Color.blue, 1, .5f);
    //     }
    //     else
    //     {
    //         TweenVignette(Color.blue, 0, .5f);
    //     }
    // }

    // void OnAbilityCast(GameObject caster, string abilityName)
    // {
    //     if(caster.CompareTag("Player"))
    //     {
    //         if(abilityName=="AOE")
    //         {
    //             FlashVignette(Color.yellow);
    //         }
    //         else
    //         {
    //             TweenVignette(Color.yellow, 1, .01f);
    //         }
    //     }
    // }

    // void OnAbilityEnd(GameObject caster, string abilityName)
    // {
    //     if(caster.CompareTag("Player"))
    //     {
    //         TweenVignette(Color.yellow, 0, .5f);
    //     }
    // }
}
