using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushAbility : BaseAbility
{
    [Header("Force Push Ability")]
    public GameObject player;

    public ForceController forcer;
    
    // ============================================================================
    
    // Anim Event
    public override void OnBaseAbilityReleaseStart()
    {
        forcer.Force(false);

        EventM.OnCastReleased(owner, abilitySO);
    }
}
