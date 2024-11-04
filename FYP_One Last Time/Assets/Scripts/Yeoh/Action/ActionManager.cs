using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowParry;
    public bool AllowCast;
    public bool AllowHurt;
    public bool AllowStun;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFlipEvent += OnTryFlip;
        EventM.TryJumpEvent += OnTryJump;
        EventM.TryJumpCutEvent += OnTryJumpCut;
        EventM.TryAutoJumpEvent += OnTryAutoJump;
        EventM.AutoJumpingEvent += OnAutoJumping;
        EventM.TryDashEvent += OnTryDash;
        EventM.TryComboEvent += OnTryCombo;
        EventM.TryRaiseParryEvent += OnTryRaiseParry;
        EventM.TryRiposteComboEvent += OnTryRiposteCombo;
        EventM.TryAbilityEvent += OnTryAbility;
        EventM.TryHurtEvent += OnTryHurt;
        EventM.TryStunEvent += OnTryStun;
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFlipEvent -= OnTryFlip;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryJumpCutEvent -= OnTryJumpCut;
        EventM.TryAutoJumpEvent -= OnTryAutoJump;
        EventM.AutoJumpingEvent -= OnAutoJumping;
        EventM.TryDashEvent -= OnTryDash;
        EventM.TryComboEvent -= OnTryCombo;
        EventM.TryRaiseParryEvent -= OnTryRaiseParry;
        EventM.TryRiposteComboEvent -= OnTryRiposteCombo;
        EventM.TryAbilityEvent -= OnTryAbility;
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

        if(!AllowJump) return;

        EventM.OnAutoJump(owner, jump_dir);
    }

    void OnAutoJumping(GameObject who, Vector3 jump_dir)
    {
        if(who!=owner) return;

        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        EventM.OnFlip(owner, dot_x);
    }

    // ============================================================================    
    
    void OnTryDash(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowDash) return;

        EventM.OnCancelFlipDelay(owner);

        EventM.OnDash(owner);
    }
    
    // ============================================================================    

    void OnTryCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;

        if(!AllowAttack) return;

        if(IsRiposteActive()) return;

        EventM.OnCancelFlipDelay(owner);

        EventM.OnCombo(owner, combo_name);
    }

    void OnTryRiposteCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;

        if(!AllowAttack) return;

        if(!IsRiposteActive()) return;

        EventM.OnCancelFlipDelay(owner);

        EventM.OnCombo(owner, combo_name);
    }  

    // ============================================================================
    
    void OnTryRaiseParry(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowParry) return;

        EventM.OnCancelFlipDelay(owner);

        EventM.OnRaiseParry(owner);
    }

    // ============================================================================    

    void OnTryAbility(GameObject who, string ability_name)
    {
        if(who!=owner) return;

        if(!AllowCast) return;

        EventM.OnCancelFlipDelay(owner);

        EventM.OnAbility(owner, ability_name);
    }

    // ============================================================================
    
    void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(!AllowHurt) return;

        EventM.OnTryParry(owner, attacker, hurtbox, contactPoint);
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

    public DashScript dash;
    public bool IsDashing() => dash?.IsPerforming() ?? false;
    
    public AttackScript attack;
    public bool IsWindingUpAttack() => attack?.IsWindingUp() ?? false;
    public bool IsReleasingAttack() => attack?.HasReleased() ?? false;
    
    public TryParryScript tryParry;
    public bool IsTryingToParry() => tryParry?.IsPerforming() ?? false;

    public OnParryScript onParry;
    public bool IsParrying() => onParry?.IsPerforming() ?? false;

    public RiposteScript riposte;
    public bool IsRiposteActive() => riposte?.IsRiposteActive() ?? false;
    
    public AbilityCaster caster;
    public bool IsCasting() => caster?.IsPerforming() ?? false;

    public HealAbility heal;
    public bool IsHealing() => heal?.IsPerforming() ?? false;

    public StunScript stun;
    public bool IsStunned() => stun?.IsPerforming() ?? false;
}
