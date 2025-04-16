using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushAbility : BaseAbility
{
    [Header("Force Push Ability")]
    public ForceAbility force;
    
    // ============================================================================
    
    // Anim Event
    public override void OnBaseAbilityReleaseStart()
    {
        force.Force(false, abilitySO);

        EventM.OnCastReleased(owner, abilitySO);
    }
}
