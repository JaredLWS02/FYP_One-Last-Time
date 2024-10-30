using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowParry;
    public bool AllowCast;
    public bool AllowStun;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFaceXEvent += OnTryFaceX;
        EventM.TryJumpEvent += OnTryJump;
        EventM.TryJumpCutEvent += OnTryJumpCut;
        EventM.TryAutoJumpEvent += OnTryAutoJump;
        EventM.TryDashEvent += OnTryDash;
        EventM.TryComboEvent += OnTryCombo;
        EventM.TryParryEvent += OnTryParry;
        EventM.TryRiposteComboEvent += OnTryRiposteCombo;
        EventM.TryAbilityEvent += OnTryAbility;
        EventM.TryStunEvent += OnTryStun;
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFaceXEvent -= OnTryFaceX;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryJumpCutEvent -= OnTryJumpCut;
        EventM.TryAutoJumpEvent -= OnTryAutoJump;
        EventM.TryDashEvent -= OnTryDash;
        EventM.TryComboEvent -= OnTryCombo;
        EventM.TryParryEvent -= OnTryParry;
        EventM.TryRiposteComboEvent -= OnTryRiposteCombo;
        EventM.TryAbilityEvent -= OnTryAbility;
        EventM.TryStunEvent -= OnTryStun;
    }

    // ============================================================================

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        EventM.OnMove(gameObject, input);
    }

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input_x=0;

        EventM.OnFaceX(gameObject, input_x);
    }
    
    // ============================================================================

    void OnTryJump(GameObject who)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        EventM.OnJump(gameObject);
    }

    void OnTryJumpCut(GameObject who)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        EventM.OnJumpCut(gameObject);
    }

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        EventM.OnAutoJump(gameObject, jump_dir);
        
        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        EventM.OnFaceX(gameObject, dot_x);
    }

    // ============================================================================    
    
    void OnTryDash(GameObject who)
    {
        if(who!=gameObject) return;

        if(!AllowDash) return;

        EventM.OnDash(gameObject);
    }
    
    // ============================================================================    

    void OnTryCombo(GameObject who, string combo_name)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        if(IsRiposteActive()) return;

        EventM.OnCombo(gameObject, combo_name);
    }

    void OnTryRiposteCombo(GameObject who, string combo_name)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        if(!IsRiposteActive()) return;

        EventM.OnCombo(gameObject, combo_name);
    }  

    // ============================================================================
    
    void OnTryParry(GameObject who)
    {
        if(who!=gameObject) return;

        if(!AllowParry) return;

        EventM.OnParry(gameObject);
    }

    // ============================================================================    

    void OnTryAbility(GameObject who, string ability_name)
    {
        if(who!=gameObject) return;

        if(!AllowCast) return;

        EventM.OnAbility(gameObject, ability_name);
    }

    // ============================================================================

    void OnTryStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        if(!AllowStun) return;

        EventM.OnStun(gameObject, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    [Header("Checks")]
    public GroundCheck ground;

    public bool IsGrounded()
    {
        if(!ground) return false;
        return ground.IsGrounded();
    }

    public AgentAutoJump autoJump;

    public bool IsAutoJumping()
    {
        if(!autoJump) return false;
        return autoJump.isJumping;
    }

    public DashScript dash;

    public bool IsDashing()
    {
        if(!dash) return false;
        return dash.IsDashingOrRecovering();
    }
    
    public AttackScript attack;

    public bool IsAttackWindingUp()
    {
        if(!attack) return false;
        return attack.isWindingUp;
    }
    public bool IsAttacking()
    {
        if(!attack) return false;
        return attack.isAttacking;
    }
    
    public ParryScript parry;

    public bool IsParryRaised()
    {
        if(!parry) return false;
        return parry.isParryRaised;
    }
    public bool IsParryLowering()
    {
        if(!parry) return false;
        return parry.isParryLowering;
    }
    public bool IsRiposteActive()
    {
        if(!parry) return false;
        return parry.IsRiposteActive();
    }
    
    public AbilityCaster caster;

    public bool IsCasting()
    {
        if(!caster) return false;
        return caster.isCasting;
    }
    public bool IsCast()
    {
        if(!caster) return false;
        return caster.isCast;
    }

    public StunScript stun;

    public bool IsStunned()
    {
        if(!stun) return false;
        return stun.isStunned;
    }
    
}
