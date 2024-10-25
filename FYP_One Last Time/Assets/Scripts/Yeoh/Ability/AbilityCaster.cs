using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public AbilityListSO abilityList;

    public HPManager MP_Manager;

    [HideInInspector]
    public bool isCasting;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CastingEvent += OnCasting;
        EventM.CastWindUpEvent += OnCastWindUp;
        EventM.CastReleaseEvent += OnCastRelease;
        EventM.CastFinishEvent += OnCastFinish;
        EventM.CastCancelEvent += OnCastCancel;

        abilityList.ResetCooldowns();
    }
    void OnDisable()
    {
        EventM.CastingEvent -= OnCasting;
        EventM.CastWindUpEvent -= OnCastWindUp;
        EventM.CastReleaseEvent -= OnCastRelease;
        EventM.CastFinishEvent -= OnCastFinish;
        EventM.CastCancelEvent -= OnCastCancel;

        abilityList.ResetCooldowns();
    }

    // Start ============================================================================
    
    public void StartCast(string ability_name)
    {
        if(isCasting) return;

        EventM.OnStartCast(gameObject, ability_name);

        AbilitySlot abilitySlot = abilityList.GetAbility(ability_name);

        // if dont have that ability
        if(abilitySlot==null) return;

        // not on cooldown
        if(abilitySlot.IsCooling()) return;

        // not enough mp
        if(MP_Manager.hp < abilitySlot.ability.cost) return;

        EventM.OnCasting(gameObject, abilitySlot);
    }

    // Casting ============================================================================

    void OnCasting(GameObject caster, AbilitySlot abilitySlot)
    {
        if(caster!=gameObject) return;

        castingRt = StartCoroutine(Casting(abilitySlot));
    }

    Coroutine castingRt;
    IEnumerator Casting(AbilitySlot abilitySlot)
    {
        isCasting=true;

        //sfxCastingLoop = AudioManager.Current.LoopSFX(gameObject, SFXManager.Current.sfxCastingLoop);

        yield return new WaitForSeconds(abilitySlot.ability.castingTime);

        EventM.OnCastWindUp(gameObject, abilitySlot);

        //if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);
    }

    // Done ============================================================================

    void OnCastWindUp(GameObject caster, AbilitySlot abilitySlot)
    {
        if(caster!=gameObject) return;

        // play ability.anim or something
        // it must have anim event to trigger EventM.OnCastRelease
    }
    
    // Release ============================================================================

    void OnCastRelease(GameObject caster, AbilitySlot abilitySlot)
    {
        if(caster!=gameObject) return;

        // mp cost
        MP_Manager.Hurt(abilitySlot.ability.cost);

        abilitySlot.DoCooldown();
    }

    // Finish ============================================================================

    void OnCastFinish(GameObject caster)
    {
        if(caster!=gameObject) return;

        isCasting=false;
    }

    // Cancel ============================================================================

    public void Cancel()
    {
        EventM.OnCastCancel(gameObject);
    }

    void OnCastCancel(GameObject caster)
    {
        if(caster!=gameObject) return;

        if(!isCasting) return;
        isCasting=false;

        if(castingRt!=null) StopCoroutine(castingRt);

        //if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);
    }

    // Misc ============================================================================

    void Update()
    {
        abilityList.UpdateCooldowns();
        abilityList.CleanUp();
    }

    /*
    //AudioSource sfxCastingLoop;

    public void OnDeath()
    {
        Cancel();
    }

    */
}
