using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullAbility : BaseAbility
{
    [Header("Force Pull Ability")]
    public ForceAbility force;
    
    // ============================================================================
    
    // Anim Event
    public override void OnBaseAbilityReleaseStart()
    {
        force.Force(true, abilitySO);

        EventM.OnCastReleased(owner, abilitySO);
    }
}
