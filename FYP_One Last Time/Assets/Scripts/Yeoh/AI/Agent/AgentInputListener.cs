using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pilot))]

public class AgentInputListener : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AgentTryMoveEvent += OnAgentTryMove;
        EventM.AgentTryFaceXEvent += OnAgentTryFaceX;
        EventM.AgentTryJumpEvent += OnAgentTryJump;
        EventM.AgentTryJumpCutEvent += OnAgentTryJumpCut;
        EventM.AgentTryAutoJumpEvent += OnAgentTryAutoJump;
        EventM.AgentTryAttackEvent += OnAgentTryAttack;
    }
    void OnDisable()
    {
        EventM.AgentTryMoveEvent -= OnAgentTryMove;
        EventM.AgentTryFaceXEvent -= OnAgentTryFaceX;
        EventM.AgentTryJumpEvent -= OnAgentTryJump;
        EventM.AgentTryJumpCutEvent -= OnAgentTryJumpCut;
        EventM.AgentTryAutoJumpEvent -= OnAgentTryAutoJump;
        EventM.AgentTryAttackEvent -= OnAgentTryAttack;
    }

    // Move ============================================================================

    Vector2 moveInput;

    void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(gameObject, moveInput);
    }

    void OnAgentTryMove(GameObject mover, Vector2 input_dir)
    {
        if(!pilot.IsAI()) return;

        if(mover!=gameObject) return;

        moveInput = input_dir;
    } 

    void OnAgentTryFaceX(GameObject facer, float dir_x)
    {
        if(!pilot.IsAI()) return;

        if(facer!=gameObject) return;

        EventM.OnTryFlip(gameObject, dir_x);
    }

    // Jump ============================================================================

    void OnAgentTryJump(GameObject jumper)
    {
        if(!pilot.IsAI()) return;

        if(jumper!=gameObject) return;

        EventM.OnTryJump(gameObject);
    }

    void OnAgentTryJumpCut(GameObject jumper)
    {
        if(!pilot.IsAI()) return;

        if(jumper!=gameObject) return;

        EventM.OnTryJump(gameObject);
    }

    void OnAgentTryAutoJump(GameObject jumper, Vector3 dir)
    {
        if(!pilot.IsAI()) return;

        if(jumper!=gameObject) return;

        EventM.OnTryAutoJump(gameObject, dir);
    }

    // Attack ============================================================================

    void OnAgentTryAttack(GameObject attacker, string type)
    {
        if(!pilot.IsAI()) return;

        if(attacker!=gameObject) return;

        EventM.OnTryCombo(gameObject, type);
    }
}
