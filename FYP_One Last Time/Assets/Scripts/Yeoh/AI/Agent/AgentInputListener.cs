using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInputListener : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AgentTryMoveEvent += OnAgentTryMove;
        EventM.AgentTryFlipEvent += OnAgentTryFlip;
        EventM.AgentTryJumpEvent += OnAgentTryJump;
        EventM.AgentTryJumpCutEvent += OnAgentTryJumpCut;
        EventM.AgentTryAutoJumpEvent += OnAgentTryAutoJump;
        EventM.AgentTryAttackEvent += OnAgentTryAttack;
    }
    void OnDisable()
    {
        EventM.AgentTryMoveEvent -= OnAgentTryMove;
        EventM.AgentTryFlipEvent -= OnAgentTryFlip;
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

        EventM.OnTryMove(owner, moveInput);
    }

    void OnAgentTryMove(GameObject who, Vector2 input_dir)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        moveInput = input_dir;
    } 

    void OnAgentTryFlip(GameObject who, float dir_x)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        EventM.OnTryFlip(owner, dir_x);
    }

    // Jump ============================================================================

    void OnAgentTryJump(GameObject who)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        EventM.OnTryJump(owner);
    }

    void OnAgentTryJumpCut(GameObject who)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        EventM.OnTryJump(owner);
    }

    void OnAgentTryAutoJump(GameObject who, Vector3 dir)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        EventM.OnTryAutoJump(owner, dir);
    }

    // Attack ============================================================================

    void OnAgentTryAttack(GameObject who, string type)
    {
        if(!pilot.IsAI()) return;

        if(who!=owner) return;

        EventM.OnTryCombo(owner, type);
    }
}
