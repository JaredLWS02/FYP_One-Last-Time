using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    public HPManager hp;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        //EventM.CastReleaseEvent += OnCastRelease;
        //temp, no anim event yet
        EventM.CastWindUpEvent += OnCastRelease;
    }
    void OnDisable()
    {
        //EventM.CastReleaseEvent -= OnCastRelease;
        //temp, no anim event yet
        EventM.CastWindUpEvent -= OnCastRelease;
    }

    // ============================================================================

    void OnCastRelease(GameObject caster, AbilitySlot abilitySlot)
    {
        if(caster!=gameObject) return;

        if(abilitySlot.ability.name!="Heal") return;

        hp.Add(abilitySlot.ability.magnitude);

        //temp, no anim event yet
        EventM.OnCastRelease(gameObject, abilitySlot);
        EventM.OnCastFinish(gameObject);

        TempFeedback(abilitySlot);
    }

    // Move to vfx manager later ============================================================================

    void TempFeedback(AbilitySlot abilitySlot)
    {
        // flash green
        SpriteManager.Current.FlashColor(gameObject, -1, 1, -1);
        ModelManager.Current.FlashColor(gameObject, -1, 1, -1);

        Vector3 top = ColliderManager.Current.GetTop(gameObject);

        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySlot.ability.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
