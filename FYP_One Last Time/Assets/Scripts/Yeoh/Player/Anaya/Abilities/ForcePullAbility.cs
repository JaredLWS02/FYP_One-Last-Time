using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullAbility : BaseAbility
{
    [Header("Force Pull Ability")]
    public GameObject player;

    public ForceController forcer;
    
    // ============================================================================
    
    // Anim Event
    public override void OnBaseAbilityReleaseStart()
    {
        forcer.Force(true);

        EventM.OnCastReleased(owner, abilitySO);
    }
}
