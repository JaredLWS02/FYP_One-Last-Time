using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : BaseAction
{
    [Header("Base Ability")]
    public AbilityCaster caster;

    // ============================================================================

    protected EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AbilityEvent += OnAbility;
        EventM.CastEvent += OnCast;
        EventM.CancelCastEvent += OnCancelCast;

    }
    void OnDisable()
    {
        EventM.AbilityEvent -= OnAbility;
        EventM.CastEvent -= OnCast;
        EventM.CancelCastEvent -= OnCancelCast;
    }

    // ============================================================================
    
    public AbilitySO abilitySO;

    void OnAbility(GameObject who, string ability_name)
    {
        if(who!=owner) return;

        if(ability_name != abilitySO.Name) return;

        if(IsCasting()) return;

        caster.abilitySO = abilitySO;

        caster.TryStartCasting();
    }
    
    // ============================================================================

    public bool IsCasting() => caster.IsPerforming() && IsPerforming();

    // ============================================================================
    
    public AbilitySlot abilitySlot;
    
    void OnCast(GameObject caster, AbilitySlot slot)
    {
        if(caster!=owner) return;

        if(slot.ability != abilitySO) return;

        if(IsPerforming()) return;
        
        abilitySlot = slot;

        Perform(slot.ability.castAnim);
    }

    // ============================================================================

    public HPManager mpM;

    // Anim Event
    public override void OnAnimReleaseStart()
    {
        mpM.Deplete(abilitySO.mpCost);

        abilitySlot.DoCooldown();

        OnBaseAbilityReleaseStart();
    }

    public virtual void OnBaseAbilityReleaseStart(){}

    // Cancel ============================================================================
    
    void OnCancelCast(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();
        
        EventM.OnCastCancelled(owner);
    }
}
