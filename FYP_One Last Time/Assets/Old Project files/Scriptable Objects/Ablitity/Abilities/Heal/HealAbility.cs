using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    public HPManager hp;

    // Event Manager ============================================================================

    void OnEnable()
    {
        //EventManager.Current.CastReleaseEvent += OnCastRelease;
        //temp, no anim event yet
        EventManager.Current.CastWindUpEvent += OnCastRelease;
    }
    void OnDisable()
    {
        //EventManager.Current.CastReleaseEvent -= OnCastRelease;
        //temp, no anim event yet
        EventManager.Current.CastWindUpEvent -= OnCastRelease;
    }

    // Events ============================================================================

    void OnCastRelease(GameObject caster, AbilitySlot abilitySlot)
    {
        if(caster!=gameObject) return;

        if(abilitySlot.ability.name!="Heal") return;

        hp.Add(abilitySlot.ability.magnitude);

        //temp, no anim event yet
        EventManager.Current.OnCastRelease(gameObject, abilitySlot);
        EventManager.Current.OnCastFinish(gameObject);

        TempFeedback(abilitySlot);
    }

    // Move to vfx manager later ============================================================================

    void TempFeedback(AbilitySlot abilitySlot)
    {
        // flash sprite green
        SpriteManager.Current.FlashColor(gameObject, -1, 1, -1);

        Vector3 top = SpriteManager.Current.GetColliderTop(gameObject);

        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySlot.ability.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
