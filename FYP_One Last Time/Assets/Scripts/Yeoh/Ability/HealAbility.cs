using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilityCaster))]

public class HealAbility : MonoBehaviour
{
    public GameObject owner;
    public HPManager hpM;    

    // ============================================================================
    
    AbilityCaster caster;

    void Awake()
    {
        caster = GetComponent<AbilityCaster>();
    }
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AbilityEvent += OnAbility;
        EventM.CastReleasedEvent += OnCastReleased;
    }
    void OnDisable()
    {
        EventM.AbilityEvent -= OnAbility;
        EventM.CastReleasedEvent -= OnCastReleased;
    }

    // ============================================================================
    
    [Header("Heal")]
    public AbilitySO healSO;

    void OnAbility(GameObject who, string ability_name)
    {
        if(who!=owner) return;

        if(ability_name != healSO.Name) return;

        if(IsCasting()) return;

        StartCasting();
    }

    void StartCasting()
    {
        caster.abilitySO = healSO;

        caster.TryStartCasting();
    }
    
    // ============================================================================

    bool IsCasting()
    {
        return caster.IsCasting();
    }

    // ============================================================================

    void OnCastReleased(GameObject caster, AbilitySO abilitySO)
    {
        if(caster!=owner) return;

        if(abilitySO!=healSO) return;

        hpM.Add(abilitySO.magnitude);

        TempVFX(abilitySO);
    }

    // Move to vfx manager later ============================================================================

    void TempVFX(AbilitySO abilitySO)
    {
        // flash green
        SpriteManager.Current.FlashColor(owner, -1, 1, -1);
        ModelManager.Current.FlashColor(owner, -1, 1, -1);

        Vector3 top = ColliderManager.Current.GetTop(owner);

        // pop up text
        VFXManager.Current.SpawnPopUpText(top, $"+{abilitySO.magnitude}", Color.green);

        //DisableCastTrails();

        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        //AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }
}
