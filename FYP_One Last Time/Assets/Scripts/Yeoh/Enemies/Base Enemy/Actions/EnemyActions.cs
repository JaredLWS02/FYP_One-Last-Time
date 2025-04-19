using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowAutoJump;
    public bool AllowAttack;
    public bool AllowHurt;
    public bool AllowStun;

    // ============================================================================

    protected EventManager EventM;

    protected virtual void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFlipEvent += OnTryFlip;
        EventM.TryJumpEvent += OnTryJump;
        EventM.TryJumpCutEvent += OnTryJumpCut;
        EventM.TryAutoJumpEvent += OnTryAutoJump;
        EventM.AutoJumpingEvent += OnAutoJumping;
        EventM.TryComboEvent += OnTryCombo;
        EventM.TryHurtEvent += OnTryHurt;
        EventM.TryStunEvent += OnTryStun;
    }
    protected virtual void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFlipEvent -= OnTryFlip;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryJumpCutEvent -= OnTryJumpCut;
        EventM.TryAutoJumpEvent -= OnTryAutoJump;
        EventM.AutoJumpingEvent -= OnAutoJumping;
        EventM.TryComboEvent -= OnTryCombo;
        EventM.TryHurtEvent -= OnTryHurt;
        EventM.TryStunEvent -= OnTryStun;
    }
        
    // ============================================================================

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        EventM.OnMove(owner, input);
    }

    void OnTryFlip(GameObject who, float input_x)
    {
        if(who!=owner) return;
        if(!AllowMoveX) return;

        EventM.OnFlip(owner, input_x);
    }
    
    // ============================================================================

    void OnTryJump(GameObject who)
    {
        if(who!=owner) return;
        if(!AllowJump) return;

        EventM.OnJump(owner);
    }

    void OnTryJumpCut(GameObject who)
    {
        if(who!=owner) return;
        if(!AllowJump) return;

        EventM.OnJumpCut(owner);
    }

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=owner) return;
        if(!AllowAutoJump) return;

        EventM.OnAutoJump(owner, jump_dir);
    }

    void OnAutoJumping(GameObject who, Vector3 jump_dir)
    {
        if(who!=owner) return;

        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        EventM.OnFlip(owner, dot_x);
    }
    
    // ============================================================================    

    void OnTryCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;
        if(!AllowAttack) return;

        EventM.OnCombo(owner, combo_name);
    }

    // ============================================================================
        
    protected virtual void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;
        if(!AllowHurt) return;

        //EventM.OnTryParry(owner, attacker, hurtbox, contactPoint);
        EventM.OnHurt(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    void OnTryStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;
        if(!AllowStun) return;

        EventM.OnStun(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    [Header("Check Action States")]
    // ?? operator means that if null, it will choose the other option
    // in this case, if null, choose false
    public GroundCheck ground;
    public bool IsGrounded() => ground?.IsGrounded() ?? false;

    public AgentAutoJump autoJump;
    public bool IsAutoJumping() => autoJump?.isJumping ?? false;

    public AttackScript attack;
    public bool IsWindingUpAttack() => attack?.IsWindingUp() ?? false;
    public bool IsReleasingAttack() => attack?.HasReleased() ?? false;
    
    public StunScript stun;
    public bool IsStunned() => stun?.IsPerforming() ?? false;

    // ============================================================================

    public void TryJump()
    {
        EventM.OnAgentTryJump(owner);
    }
}
