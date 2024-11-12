using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public string attackName = "Normal Combo";

    public void Attack()
    {
        if(CanSeeTarget())
        EventM.OnAgentTryAttack(owner, attackName);
    }

    // ============================================================================
    
    [Header("Sight")]
    public BaseRaycast sight;
    public bool CanSeeTarget() => sight?.IsHitting() ?? true;

}
