using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : BaseAbility
{
    [Header("Heal Ability")]
    public GameObject player;
    public HPManager hpM;
    
    // ============================================================================
    
    // Anim Event
    public override void OnBaseAbilityReleaseStart()
    {
        hpM.Add(abilitySO.magnitude);

        EventM.OnHeal(player, owner, abilitySO.magnitude);

        EventM.OnCastReleased(owner, abilitySO);
    }
}
