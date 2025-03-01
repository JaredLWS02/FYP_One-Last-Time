using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyviActionInputs : EnemyActionInputs
{
    protected override void OnEnable2()
    {
        EventM.AgentTryParryEvent += OnAgentTryParry;
    }
    protected override void OnDisable2()
    {
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
