using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnayaActions : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================
    
    [Header("Toggles")]
    public bool AllowAbility;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryAbilityEvent += OnTryAbility;
    }
    void OnDisable()
    {
        EventM.TryAbilityEvent -= OnTryAbility;
    }

    // ============================================================================

    void OnTryAbility(GameObject who, string ability_name)
    {
        if(who!=owner) return;

        if(!AllowAbility) return;

        EventM.OnAbility(owner, ability_name);
    }

    // ============================================================================

    [Header("Check Action States")]
    
    public AbilityCaster caster;
    public bool IsCasting() => caster?.IsPerforming() ?? false;

    public HealAbility heal;
    public bool IsHealing() => heal?.IsPerforming() ?? false;

    public ForcePullAbility pull;
    public bool IsForcePulling() => pull?.IsPerforming() ?? false;

    public ForcePushAbility push;
    public bool IsForcePushing() => push?.IsPerforming() ?? false;

    public bool IsDoingAbility() => IsHealing() || IsForcePulling() || IsForcePushing();
}
