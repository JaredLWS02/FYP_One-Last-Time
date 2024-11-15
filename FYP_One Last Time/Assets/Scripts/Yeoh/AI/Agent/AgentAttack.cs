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
        if(!IsGroundCheckValid()) return;
        if(!CanSeeTarget()) return;

        EventM.OnAgentTryAttack(owner, attackName);
    }

    // ============================================================================
    
    [Header("Optional")]
    public GroundCheck ground;
    public bool allowGrounded=true;
    public bool allowMidAir=true;

    public bool IsGroundCheckValid()
    {
        if(!ground) return true;

        return (allowGrounded && ground.IsGrounded()) ||
                (allowMidAir && !ground.IsGrounded());
    }

    public BaseRaycast sight;
    public bool CanSeeTarget() => sight?.IsHitting() ?? true;

}
