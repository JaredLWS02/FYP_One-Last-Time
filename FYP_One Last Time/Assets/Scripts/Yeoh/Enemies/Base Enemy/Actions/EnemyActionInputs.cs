using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionInputs : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // ============================================================================

    protected EventManager EventM;

    protected virtual void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.AgentTryMoveEvent += OnAgentTryMove;
        EventM.AgentTryFlipEvent += OnAgentTryFlip;
        EventM.AgentTryJumpEvent += OnAgentTryJump;
        EventM.AgentTryJumpCutEvent += OnAgentTryJumpCut;
        EventM.AgentTryAutoJumpEvent += OnAgentTryAutoJump;
        EventM.AgentTryAttackEvent += OnAgentTryAttack;
    }
    protected virtual void OnDisable()
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

    protected virtual void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(owner, moveInput);
    }

    void OnAgentTryMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        moveInput = input;
    } 

    void OnAgentTryFlip(GameObject who, float dir_x)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryFlip(owner, dir_x);
    }

    // Jump ============================================================================

    void OnAgentTryJump(GameObject who)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryJump(owner);
    }

    void OnAgentTryJumpCut(GameObject who)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryJumpCut(owner);
    }

    void OnAgentTryAutoJump(GameObject who, Vector3 dir)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryAutoJump(owner, dir);
    }

    // Attack ============================================================================

    void OnAgentTryAttack(GameObject who, string type)
    {
        if(who!=owner) return;
        if(!pilot.IsAI()) return;

        EventM.OnTryCombo(owner, type);
    }
    
}
