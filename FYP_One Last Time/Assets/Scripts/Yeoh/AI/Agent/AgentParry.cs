using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentParry : MonoBehaviour
{
    [Header("Agent Parry")]
    public GameObject owner;

    public BaseOverlap overlap;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.AttackWindedUpEvent += OnAttackWindedUp;
    }
    void OnDisable()
    {
        EventM.AttackWindedUpEvent -= OnAttackWindedUp;
    }
    
    // ============================================================================

    [Header("Chance")]
    [Range(0,100)]
    public float parryChance = 50;

    void OnAttackWindedUp(GameObject attacker, AttackSO attackSO)
    {
        if(attacker == owner) return; // ignore if self
        if(!IsGroundCheckValid()) return;
        if(!CanSeeTarget()) return;
        if(Random.Range(0,100) > parryChance) return;
        overlap.Check();
        if(!overlap.IsOverlappingWho(attacker)) return;

        EventM.OnAgentTryParry(owner);
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
