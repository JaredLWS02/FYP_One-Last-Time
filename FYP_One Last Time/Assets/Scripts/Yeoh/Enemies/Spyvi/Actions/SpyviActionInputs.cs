using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyviActionInputs : EnemyActionInputs
{
    protected override void OnEnable()
    {
        base.OnEnable();

        EventM.AgentTryParryEvent += OnAgentTryParry;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        
        EventM.AgentTryParryEvent -= OnAgentTryParry;
    }

    // Parry ============================================================================
    
    void OnAgentTryParry(GameObject who)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryRaiseParry(owner);
    }
}
